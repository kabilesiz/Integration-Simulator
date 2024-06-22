using Integration.Models;

namespace Integration.Helpers;

public static class TableHelper
{
    public static void ShowResults(List<TableData> entryOrderTableData, string entryOrderTableTitle, 
        List<TableData> finalOrderTableData, string finalOrderTableTitle, string[] columns)
    {
        var columnWidths = GetColumnWidths(entryOrderTableData, columns);

        var maxRows = Math.Max(entryOrderTableData.Count, finalOrderTableData.Count);

        PrintLine(columnWidths);
        PrintHeaders(columnWidths, entryOrderTableTitle, finalOrderTableTitle);
        PrintLine(columnWidths);
        PrintColumns(columnWidths, columns);
        PrintLine(columnWidths);

        for (var i = 0; i < maxRows; i++)
        {
            var entryOrderTableDataRow = i < entryOrderTableData.Count ? entryOrderTableData[i] : null;
            var finalOrderTableDataRow = i < finalOrderTableData.Count ? finalOrderTableData[i] : null;
            
            PrintRow(entryOrderTableDataRow, finalOrderTableDataRow, columnWidths);
            PrintLine(columnWidths);
        }
    }

    private static void PrintHeaders(int[] columnWidths, string entryOrderTableTitle, string finalOrderTableTitle)
    {
        var totalWidth = columnWidths.Sum(width => width + 3) - 1; 

        var padding = (totalWidth - entryOrderTableTitle.Length) / 2;

        var paddedEntryOrderTableTitle = new string(' ', padding) + entryOrderTableTitle +
                                         new string(' ', totalWidth - entryOrderTableTitle.Length - padding);
        var paddedFinalOrderTableTitle = new string(' ', padding) + finalOrderTableTitle +
                                         new string(' ', totalWidth - finalOrderTableTitle.Length - padding);

        Console.WriteLine($"\t|{paddedEntryOrderTableTitle}| |{paddedFinalOrderTableTitle}|");
    }

    private static void PrintColumns(IReadOnlyList<int> columnWidths, IReadOnlyList<string> columns)
    {
        Console.Write("\t|");
        
        for (var i = 0; i < columns.Count; i++)
        {
            Console.Write(" " + columns[i].PadRight(columnWidths[i]) + " |");
        }
        
        Console.Write(" |");
        
        for (var i = 0; i < columns.Count; i++)
        {
            Console.Write(" " + columns[i].PadRight(columnWidths[i]) + " |");
        }
        
        Console.WriteLine();
    }

    private static void PrintLine(int[] columnWidths)
    {
        Console.Write("\t+");
        
        foreach (var width in columnWidths)
        {
            Console.Write(new string('-', width + 2));
            Console.Write("+");
        }
        
        Console.Write(" +");
        
        foreach (var width in columnWidths)
        {
            Console.Write(new string('-', width + 2));
            Console.Write("+");
        }
        
        Console.WriteLine();
    }

    private static void PrintRow(TableData entryOrderTableDataRow, TableData finalOrderTableDataRow, 
        IReadOnlyList<int> columnWidths)
    {
        Console.Write("\t|");
        
        if (entryOrderTableDataRow != null)
        {
            Console.Write(" " + entryOrderTableDataRow.Id.ToString().PadRight(columnWidths[0]) + " |");
            Console.Write(" " + (entryOrderTableDataRow.Content ?? string.Empty).PadRight(columnWidths[1]) + " |");
        }
        else
        {
            Console.Write(" ".PadRight(columnWidths[0] + 2) + "|");
            Console.Write(" ".PadRight(columnWidths[1] + 2) + "|");
        }

        Console.Write(" |");

        if (finalOrderTableDataRow != null)
        {
            Console.Write(" " + finalOrderTableDataRow.Id.ToString().PadRight(columnWidths[0]) + " |");
            Console.Write(" " + (finalOrderTableDataRow.Content ?? string.Empty).PadRight(columnWidths[1]) + " |");
        }
        else
        {
            Console.Write(" ".PadRight(columnWidths[0] + 2) + "|");
            Console.Write(" ".PadRight(columnWidths[1] + 2) + "|");
        }

        Console.WriteLine();
    }

    private static int[] GetColumnWidths(List<TableData> table, IReadOnlyList<string> headers)
    {
        var columns = headers.Count;
        var widths = new int[columns];

        for (var i = 0; i < columns; i++)
        {
            widths[i] = headers[i].Length;
        }

        foreach (var row in table)
        {
            if (row.Id.ToString().Length > widths[0])
            {
                widths[0] = row.Id.ToString().Length;
            }
            if (!string.IsNullOrEmpty(row.Content) && row.Content.Length > widths[1])
            {
                widths[1] = row.Content.Length;
            }
        }

        return widths;
    }
}