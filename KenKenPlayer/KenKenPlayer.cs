using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace KenKenPlayer
{
    public partial class KenKenPlayer : Form
    {
        public KenKenPlayer()
        {
            InitializeComponent();
        }

        private int gridSize;                                   // Number of cells in each row and column.
        private List<List<Cell>> cageAssignments = new List<List<Cell>>();
        private List<Cage> cageEntries = new List<Cage>();
        private int cellSize = 125;                             // Determines how big each cell is. Visible size will be less because of the width of the border and grid lines.
        private Color borderColor = Color.Gray;                 // Color used for the outer border lines and heavy cage lines.
        private Color gridColor = Color.Gray;                   // Color used for the grid lines.
        private int topLeftCoord = 54;                          // Offset from the top-left corner of the canvas.
        private int bottomRightCoord;                           // Counterpart to topLeftCoord. Not known until the grid size is known.
        private int borderWidth = 5;                            // Width of the outer border lines and heavy cage lines.
        private int gridWidth = 1;                              // Width of the grid lines.
        private Color puzzleBackgroundColor = Color.Black;      // Background color of the puzzle canvas.
        private Color foregroundColor = Color.White;            // Color used for most text elements.
        private Font cornerTextFont = new Font("Arial", 20);    // Font used for the corner text with the result and operation.
        private Brush cornerTextColor = Brushes.White;          // Color used for the corner text.
        private Bitmap puzzleCanvas;                            // Bitmap for the puzzle canvas.
        private Regex cageRegex = new Regex(@"(?<index>[\dA-F]+):\s+(?<result>\d+)\s*(?<operation>[+\-/x*_])?", RegexOptions.IgnoreCase);
        private Font textBoxFont = new Font("Arial", 36);       // Font used for the cell textboxes.
        private Color textBoxIncorrectColor = Color.Red;        // Color used to show incorrect cells.
        private TextBox focusedTextBox;                         // Used to track the currently focused textbox so the notes textbox knows where to store its text.
        private DateTime startTime;                             // Records the time at which the puzzle has finished loading from the text file and was rendered in the program.
        private bool puzzleCompleted;                           // Indicates whether the puzzle has been completed yet or not. Used for displaying the completion time in the title bar.

        private void KenKenPlayer_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void KenKenPlayer_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, true);
            string puzzleFilePath = s[0];

            if (Path.GetExtension(puzzleFilePath).ToLower() != ".txt")
            {
                MessageBox.Show("Please drag in a .txt file.", "File error");
                return;
            }

            // Disable the Check button and Notes textbox when file parsing starts. They'll be enabled later if everything goes well.
            CheckButton.Enabled = false;
            NotesTextBox.ReadOnly = true;
            NotesTextBox.Text = string.Empty;

            // Clear the background image and display the instructions label in case parsing fails.
            BackgroundImage = null;
            InstructionsLabel.Visible = true;

            // Remove any existing textboxes before adding new ones.
            RemoveExistingCellTextBoxes();

            // Once the textboxes have been removed, it's safe to clear out the old cage assignments and entries.
            cageAssignments.Clear();
            cageEntries.Clear();

            // Read the provided .txt file and attempt to parse its data into cage assignments and cage entries.
            try
            {
                GetPuzzleDataFromFile(puzzleFilePath);
            }
            catch (FormatException formatExcept)
            {
                MessageBox.Show($"There was an error when parsing the puzzle file:\n\n{formatExcept.Message}", "File parse error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Perform a cross-check between the cage assignments and the cage entries to make sure the data lines up.
            if (!CrossCheckCages())
            {
                MessageBox.Show("There was an error when parsing the puzzle file.\n\nSome of the cages from the first half of the text file do not line up with the second half. Please check the text file.", "File parse error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // If the puzzle file has been parsed successfully, everything should be fine. Hide the instructions label now.
            InstructionsLabel.Visible = false;

            // With the puzzle size known, prepare the canvas for drawing operations.
            puzzleCanvas = new Bitmap((gridSize + 1) * cellSize, (gridSize + 1) * cellSize);

            // Set the background of the Form itself to match the background color of the puzzle canvas to make it blend in.
            BackColor = puzzleBackgroundColor;

            // Draw the regular grid lines.
            DrawGridLines();

            // Draw the corner text.
            DrawCornerText();

            // Draw the heavy cage borders.
            DrawHeavyBorders(FindHeavyCageBorders(cageAssignments), false);
            DrawHeavyBorders(FindHeavyCageBorders(RotateColumnsToRows(cageAssignments)), true);

            // Draw the outer borders.
            DrawOuterBorders();

            // Show the fully rendered puzzle in the Form.
            BackgroundImage = puzzleCanvas;

            // Resize the Form window to reflect the size of the puzzle canvas.
            Width = puzzleCanvas.Width;
            Height = puzzleCanvas.Height;

            // Update the title of the Form with the name of the puzzle file.
            Text = $"KenKen Player ({Path.GetFileName(puzzleFilePath)})";

            // Make new textboxes for each cell.
            CreateNewCellTextBoxes();

            // Enable the Check button.
            CheckButton.Enabled = true;

            // Record the start time.
            startTime = DateTime.Now;
            puzzleCompleted = false;

            // Set the focus to the first cell.
            cageAssignments.First().First().textBox.Focus();
        }

        private void GetPuzzleDataFromFile(string filePath)
        {
            string[] fileData = File.ReadAllLines(filePath);

            // First, get the cage assignments, which come from the first half of the puzzle file.

            // Make sure the file isn't empty.
            if (fileData.Length == 0)
            {
                throw new FormatException("File appears to be empty. Please check the file.");
            }

            // Get the dimensions of the puzzle by reading the first line and splitting it on commas to get the number of cells in the first row.
            gridSize = fileData[0].Split(',').Length;

            // Perform the first form of validation here by ensuring there are commas on the first line.
            if (gridSize == 1)
            {
                throw new FormatException("Could not determine puzzle dimensions. Please check the first line in the text file.");
            }

            bottomRightCoord = gridSize * cellSize + topLeftCoord;

            // Now read that many lines from the file and split each line on commas to get the cage assignments for each row.
            foreach (var rowString in fileData.Take(gridSize))
            {
                List<Cell> cells = new List<Cell>();

                int cageAssignment = 0;
                foreach (var parseSuccess in rowString.Split(',').Select(cageIndexString => int.TryParse(cageIndexString.Replace("0x", ""), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out cageAssignment)))
                {
                    // Make sure the cage assignments could be parsed as numbers.
                    if (!parseSuccess)
                    {
                        throw new FormatException($"Could not parse '{rowString}' as {gridSize} numbers. Please check the text file.");
                    }

                    Cell newCell = new Cell { cageIndex = cageAssignment };
                    cells.Add(newCell);
                }

                // Do some more validation in here by ensuring each line has the same number of entries as the first line, aka gridSize.
                if (cells.Count() != gridSize)
                {
                    throw new FormatException($"Puzzle rows do not contain {gridSize} cells as expected. Please check the text file.");
                }

                cageAssignments.Add(cells);
            }

            // Then, skip past the cage assignments to get to the cage entry data in the second half of the file.
            var matches = new List<Match>();
            foreach (var line in fileData.Skip(gridSize).Take(GetCageIndices().Count))
            {
                Match lineMatch = cageRegex.Match(line);

                // Perform validation here by checking the "index" group for success. index and result are both required, while operation is optional.
                if (!lineMatch.Groups["index"].Success)
                {
                    throw new FormatException($"Could not parse '{line}'. Please make sure it follows the specified format.");
                }

                matches.Add(lineMatch);
            }

            // Now that the regex matching is over, go through the match results and parse them into Cage objects.
            foreach (var match in matches)
            {
                var cage = new Cage();

                bool indexParseSuccess = int.TryParse(match.Groups["index"].Value.Replace("0x", ""), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out int index);
                bool resultParseSuccess = int.TryParse(match.Groups["result"].Value, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out int result);

                if (!indexParseSuccess || !resultParseSuccess)
                {
                    throw new FormatException($"Could not parse '{match}'. Please make sure it follows the specified format.");
                }

                cage.index = index;
                cage.result = result;

                // Handle the operation.
                switch (match.Groups["operation"].Value.ToLower())
                {
                    case "+":
                        cage.operation = CageOperation.Addition;
                        break;
                    case "-":
                        cage.operation = CageOperation.Subtraction;
                        break;
                    case "x":
                    case "*":
                        cage.operation = CageOperation.Multiplication;
                        break;
                    case "/":
                        cage.operation = CageOperation.Division;
                        break;
                    case "_":
                    default:
                        cage.operation = CageOperation.NoOperation;
                        break;
                }

                cageEntries.Add(cage);
            }
        }

        private bool CrossCheckCages()
        {
            HashSet<int> cageAssignmentIndices = GetCageIndices();
            HashSet<int> cageEntryIndices = new HashSet<int>(cageEntries.Select(entry => entry.index));

            return cageAssignmentIndices.SetEquals(cageEntryIndices);
        }

        private HashSet<int> GetCageIndices()
        {
            HashSet<int> cageAssignmentIndices = new HashSet<int>();

            foreach (var row in cageAssignments)
            {
                foreach (var cell in row)
                {
                    cageAssignmentIndices.Add(cell.cageIndex);
                }
            }

            return cageAssignmentIndices;
        }

        private void DrawGridLines()
        {
            using (Pen gridPen = new Pen(gridColor, gridWidth))
            using (Graphics gr = Graphics.FromImage(puzzleCanvas))
            {
                foreach (int cellIndex in Enumerable.Range(1, gridSize - 1))
                {
                    int coord = cellIndex * cellSize + topLeftCoord;
                    gr.DrawLine(gridPen, coord, topLeftCoord, coord, bottomRightCoord);
                    gr.DrawLine(gridPen, topLeftCoord, coord, bottomRightCoord, coord);
                }
            }
        }

        private List<List<Cell>> RotateColumnsToRows(List<List<Cell>> puzzleGrid)
        {
            var rotatedPuzzleGrid = new List<List<Cell>>();

            foreach (int cellIndex in Enumerable.Range(0, puzzleGrid[0].Count))
            {
                List<Cell> newColumn = new List<Cell>();

                foreach (List<Cell> row in puzzleGrid)
                {
                    newColumn.Add(row[cellIndex]);
                }

                rotatedPuzzleGrid.Add(newColumn);
            }

            return rotatedPuzzleGrid;
        }

        private List<List<bool>> FindHeavyCageBorders(List<List<Cell>> puzzleGrid)
        {
            var heavyCageBorders = new List<List<bool>>();

            foreach (List<Cell> row in puzzleGrid)
            {
                List<bool> borders = new List<bool>();

                foreach (int cellIndex in Enumerable.Range(0, row.Count - 1))
                {
                    bool heavyBorder = row[cellIndex].cageIndex != row[cellIndex + 1].cageIndex;
                    borders.Add(heavyBorder);
                }

                heavyCageBorders.Add(borders);
            }

            return heavyCageBorders;
        }

        private void DrawHeavyBorders(List<List<bool>> heavyCageBorders, bool horizontal)
        {
            foreach ((List<bool> borders, int outerIndex) in heavyCageBorders.Select((item, index) => (item, index)))
            {
                // Since everything's a square, these two coordinates can either be on the X or Y axis, representing the left/right edges or the top/bottom edges of the cell.
                int outerCoord1 = outerIndex * cellSize + topLeftCoord;
                int outerCoord2 = (outerIndex + 1) * cellSize + topLeftCoord;

                foreach ((bool borderPresent, int innerIndex) in borders.Select((item, index) => (item, index)))
                {
                    // Depending on if horizontal is true or not, the outer coordinates and inner coordinates will swap between being X and Y coordinates.
                    int innerCoord = innerIndex * cellSize + cellSize + topLeftCoord;

                    if (borderPresent)
                    {
                        using (Pen borderPen = new Pen(borderColor, borderWidth))
                        using (Graphics gr = Graphics.FromImage(puzzleCanvas))
                        {
                            if (horizontal)
                            {
                                gr.DrawLine(borderPen, outerCoord1, innerCoord, outerCoord2, innerCoord);
                            }
                            else
                            {
                                gr.DrawLine(borderPen, innerCoord, outerCoord1, innerCoord, outerCoord2);
                            }
                        }
                    }
                }
            }
        }

        private (int row, int cell) FindTopLeftCell(List<List<Cell>> puzzleGrid, int cageIndex)
        {
            foreach ((List<Cell> cells, int index) row in puzzleGrid.Select((row, index) => (row, index)))
            {
                foreach (var cell in row.cells.Select((cageAssignment, index) => (cageAssignment, index)))
                {
                    if (cell.cageAssignment.cageIndex == cageIndex)
                    {
                        return (row.index, cell.index);
                    }
                }
            }

            // This really shouldn't happen because the cages are cross-checked between the cage assignments and the cage entries, but just in case.
            throw new KeyNotFoundException($"Couldn't find cells belonging to cage {cageIndex}.");
        }

        private void DrawCornerText()
        {
            foreach (var cage in cageEntries)
            {
                // Find the top-left cell for the given cage.
                var (row, cell) = FindTopLeftCell(cageAssignments, cage.index);

                // Calculate the coordinates for the cell.
                (int topLeftXCoord, int topLeftYCoord) = GetCellCoordinates(row, cell);
                Rectangle cellRectangle = new Rectangle(topLeftXCoord, topLeftYCoord, cellSize, cellSize);

                // Draw the corner text in the corner of the cell.
                using (Graphics gr = Graphics.FromImage(puzzleCanvas))
                {
                    gr.DrawString(cage.ToString(), cornerTextFont, cornerTextColor, cellRectangle);
                }
            }
        }

        private (int xCoord, int yCoord) GetCellCoordinates(int rowIndex, int cellIndex)
        {
            return (topLeftCoord + (cellIndex * cellSize), topLeftCoord + (rowIndex * cellSize));
        }

        private void DrawOuterBorders()
        {
            Rectangle outerBorderRectangle = Rectangle.FromLTRB(topLeftCoord, topLeftCoord, bottomRightCoord, bottomRightCoord);

            using (Pen borderPen = new Pen(borderColor, borderWidth))
            using (Graphics gr = Graphics.FromImage(puzzleCanvas))
            {
                gr.DrawRectangle(borderPen, outerBorderRectangle);
            }
        }

        private void RemoveExistingCellTextBoxes()
        {
            var textBoxes = Controls.OfType<TextBox>().ToList();
            foreach (TextBox textBox in textBoxes)
            {
                textBox.TextChanged -= ValidateCellTextBoxValue;
                textBox.GotFocus -= CellTextBox_GotFocus;
                textBox.KeyDown -= CellTextBox_KeyDown;
                Controls.Remove(textBox);
                Controls.Remove(((TextBoxTagData)textBox.Tag).notesPreview);
                textBox.Dispose();
            }
        }

        private void CreateNewCellTextBoxes()
        {
            foreach (var row in cageAssignments.Select((cells, index) => (cells, index)))
            {
                foreach (var cell in row.cells.Select((cell, index) => (cell, index)))
                {
                    (int xCoord, int yCoord) = GetCellCoordinates(row.index, cell.index);

                    TextBox newTextBox = new TextBox
                    {
                        BackColor = puzzleBackgroundColor,
                        BorderStyle = BorderStyle.None,
                        Font = textBoxFont,
                        ForeColor = foregroundColor,
                        MaxLength = 1,
                        TextAlign = HorizontalAlignment.Center,
                        Name = $"{row.index},{cell.index}",
                        Tag = new TextBoxTagData()
                    };
                    newTextBox.TextChanged += ValidateCellTextBoxValue;
                    newTextBox.GotFocus += CellTextBox_GotFocus;
                    newTextBox.KeyDown += CellTextBox_KeyDown;

                    // Setting the size of a textbox is generally a fruitless effort because it just ignores it and automatically sets its size based on the font size.
                    int textBoxXCoord = xCoord + (cellSize / 2) - (newTextBox.Size.Width / 2);
                    int textBoxYCoord = yCoord + (cellSize / 2) - (newTextBox.Size.Height / 2);
                    newTextBox.Location = new Point(textBoxXCoord, textBoxYCoord);

                    int richTextBoxWidth = (int)(cellSize * 0.90);
                    int richTextBoxHeight = 20;
                    int richTextBoxXCoord = xCoord + ((cellSize - richTextBoxWidth) / 2);
                    int richTextBoxYCoord = yCoord + cellSize - (int)(richTextBoxHeight * 1.2);

                    RichTextBox newRichTextBox = new RichTextBox
                    {
                        BackColor = puzzleBackgroundColor,
                        ForeColor = foregroundColor,
                        Size = new Size(richTextBoxWidth, richTextBoxHeight),
                        Location = new Point(richTextBoxXCoord, richTextBoxYCoord),
                        Multiline = false,
                        BorderStyle = BorderStyle.None,
                        ReadOnly = true,
                        TabStop = false
                    };
                    ((TextBoxTagData)newTextBox.Tag).notesPreview = newRichTextBox;

                    Controls.Add(newTextBox);
                    Controls.Add(newRichTextBox);
                    cell.cell.textBox = newTextBox;
                }
            }
        }

        private void ValidateCellTextBoxValue(object sender, EventArgs e)
        {
            if (!int.TryParse(((TextBox)sender).Text, out int textBoxValue) || 0 >= textBoxValue || textBoxValue > gridSize)
            {
                ((TextBox)sender).Text = string.Empty;
            }

            ClearAllCellHighlighting();
        }

        private void CellTextBox_GotFocus(object sender, EventArgs e)
        {
            focusedTextBox = (TextBox)sender;
            NotesTextBox.ReadOnly = false;
            NotesTextBox.Rtf = ((TextBoxTagData)focusedTextBox.Tag).notes;
            focusedTextBox.SelectAll();
        }

        private void CellTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.N)
            {
                e.SuppressKeyPress = true;
                NotesTextBox.Focus();
            }
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            if (cageAssignments.Count == 0)
            {
                return;
            }

            // Clear the highlighting from the rows.
            ClearAllCellHighlighting();

            // First, validate the rows.
            foreach (var row in cageAssignments.Select((row, index) => (row, index)))
            {
                if (!CheckRow(row.row))
                {
                    HighlightRow(row.index, false, true);
                    return;
                }
            }

            // And then the columns.
            foreach (var col in RotateColumnsToRows(cageAssignments).Select((col, index) => (col, index)))
            {
                if (!CheckRow(col.col))
                {
                    HighlightRow(col.index, true, true);
                    return;
                }
            }

            // Gather up the Cages and their corresponding cells.
            var cagesWithCells = new Dictionary<int, List<Cell>>();
            foreach (var row in cageAssignments)
            {
                foreach (var cell in row)
                {
                    if (cagesWithCells.ContainsKey(cell.cageIndex))
                    {
                        cagesWithCells[cell.cageIndex].Add(cell);
                    }
                    else
                    {
                        cagesWithCells[cell.cageIndex] = new List<Cell> { cell };
                    }
                }
            }

            // Perform the operation for each Cage and compare the calculated results with the expected results.
            var cageResults = new Dictionary<int, bool>();
            foreach (var kvp in cagesWithCells)
            {
                int result = PerformOperationOnCage(kvp.Value, cageEntries.Where(entry => entry.index == kvp.Key).First().operation);
                cageResults[kvp.Key] = result == cageEntries.Where(entry => entry.index == kvp.Key).First().result;
            }

            // If any of the calculated results did not match the expected results, the cage in question contains incorrect values. Otherwise, the puzzle was completed successfully.
            if (cageResults.ContainsValue(false))
            {
                foreach (var item in cageResults.Where(result => result.Value == false))
                {
                    HighlightCells(cagesWithCells[item.Key], true);
                }
            }
            else
            {
                PlayVictoryAnimation();

                if (!puzzleCompleted)
                {
                    puzzleCompleted = true;
                    Text += $" {DateTime.Now - startTime}";
                }
            }
        }

        private bool CheckRow(List<Cell> row)
        {
            int minNumber = 1;
            int maxNumber = row.Count;

            // Gather the numbers from the row to make things a bit easier later. In this case, that means getting the text from the textboxes and parsing it into ints.
            List<int> rowNumbers = new List<int>();
            foreach (var cell in row)
            {
                // If the textbox is empty, return false immediately. It's obviously not going to validate.
                if (string.IsNullOrWhiteSpace(cell.textBox.Text))
                {
                    return false;
                }

                rowNumbers.Add(int.Parse(cell.textBox.Text));
            }

            // Make sure the row contains all the numbers it's supposed to, and only one of each
            if (!rowNumbers.OrderBy(number => number).SequenceEqual(Enumerable.Range(minNumber, maxNumber)))
            {
                return false;
            }

            // If it hasn't returned false by now, the row is probably fine.
            return true;
        }

        private int PerformOperationOnCage(List<Cell> cells, CageOperation operation)
        {
            int result = 0;

            // Gather the numbers from the row and parse them into ints once again.
            List<int> numbers = new List<int>();
            foreach (var cell in cells)
            {
                // If the textbox is empty, return -1 immediately. It's obviously not going to validate.
                if (string.IsNullOrWhiteSpace(cell.textBox.Text))
                {
                    return -1;
                }

                numbers.Add(int.Parse(cell.textBox.Text));
            }

            switch (operation)
            {
                case CageOperation.Addition:
                    result = 0;
                    break;
                case CageOperation.Multiplication:
                    result = 1;
                    break;
                case CageOperation.Subtraction:
                case CageOperation.Division:
                    numbers.Sort();
                    numbers.Reverse();
                    result = numbers[0];
                    numbers = numbers.Skip(1).ToList();
                    break;
                case CageOperation.NoOperation:
                    return numbers[0];
            }

            foreach (var number in numbers)
            {
                switch (operation)
                {
                    case CageOperation.Addition:
                        result += number;
                        break;
                    case CageOperation.Subtraction:
                        result -= number;
                        break;
                    case CageOperation.Multiplication:
                        result *= number;
                        break;
                    case CageOperation.Division:
                        result /= number;
                        break;
                }
            }

            return result;
        }

        private void ClearAllCellHighlighting()
        {
            foreach (var row in cageAssignments)
            {
                HighlightCells(row, false);
            }
        }

        private void HighlightRow(int rowIndex, bool column, bool highlight)
        {
            var cages = cageAssignments;

            if (column)
            {
                cages = RotateColumnsToRows(cages);
            }

            HighlightCells(cages[rowIndex], highlight);
        }

        private void HighlightCells(List<Cell> cells, bool highlight)
        {
            Color cellColor = highlight ? textBoxIncorrectColor : puzzleBackgroundColor;

            foreach (var cell in cells)
            {
                cell.textBox.BackColor = cellColor;
            }
        }

        private void PlayVictoryAnimation()
        {
            foreach (var item in new List<Color> { Color.Lime, Color.LimeGreen, Color.Green }.Select((color, index) => (color, index)))
            {
                new Thread(() => VictoryAnimationCycle(item.index, item.color)).Start();
            }
        }

        private void VictoryAnimationCycle(int sequenceIndex, Color color)
        {
            Thread.Sleep(sequenceIndex * 100);
            foreach (var row in cageAssignments)
            {
                foreach (var cell in row)
                {
                    cell.textBox.BackColor = color;
                    Thread.Sleep(100);
                }
            }
        }

        private void NotesTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBoxTagData tagData = (TextBoxTagData)focusedTextBox.Tag;
            tagData.notes = NotesTextBox.Rtf;
            tagData.notesPreview.Rtf = NotesTextBox.Rtf;
            tagData.notesPreview.SelectAll();
            tagData.notesPreview.SelectionColor = foregroundColor;
            tagData.notesPreview.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void NotesTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var fontStyleShortcuts = new Dictionary<Keys, FontStyle>
            {
                { Keys.B, FontStyle.Bold },
                { Keys.I, FontStyle.Italic },
                { Keys.U, FontStyle.Underline },
                { Keys.K, FontStyle.Strikeout }
            };

            if (e.Control)
            {
                if (e.KeyCode == Keys.N)
                {
                    e.SuppressKeyPress = true;
                    focusedTextBox.Focus();
                }
                else if (fontStyleShortcuts.ContainsKey(e.KeyCode))
                {
                    e.SuppressKeyPress = true;
                    NotesTextBox.SelectionFont = new Font(NotesTextBox.SelectionFont, NotesTextBox.SelectionFont.Style ^ fontStyleShortcuts[e.KeyCode]);
                }
            }
        }
    }

    public enum CageOperation
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
        NoOperation
    }

    public class Cage
    {
        public int index;
        public int result;
        public CageOperation operation;

        public override string ToString()
        {
            char operationCharacter = ' ';

            switch (operation)
            {
                case CageOperation.Addition:
                    operationCharacter = '+';
                    break;
                case CageOperation.Subtraction:
                    operationCharacter = '−';
                    break;
                case CageOperation.Multiplication:
                    operationCharacter = '×';
                    break;
                case CageOperation.Division:
                    operationCharacter = '÷';
                    break;
                case CageOperation.NoOperation:
                    break;
            }

            return $"{result}{operationCharacter}";
        }
    }

    public class Cell
    {
        public int cageIndex;
        public TextBox textBox;
    }

    public class TextBoxTagData
    {
        public string notes;
        public RichTextBox notesPreview;
    }
}
