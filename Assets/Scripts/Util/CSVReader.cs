using UnityEngine;
using System.Collections;

public class CSVReader {
	
	public static string [][] ParseTSV (TextAsset TSV) {
		return ParseCSV(TSV, '\t');
	}

	// Returns a 2d array of the CSV
	public static string [][] ParseCSV (TextAsset CSV, char separator = ',', bool removeSpaces = true) {
		if (CSV == null) {
			return null;
		}

		string[] CSVByLine = CSV.text.Split('\n');
		string[][] CSVByCell = new string[CSVByLine.Length][];

		for (int i = 0; i < CSVByLine.Length; i++) {
			CSVByCell[i] = CSVByLine[i].Split(separator);

			if (removeSpaces) {
				for (int j = 0; j < CSVByCell[i].Length; j++) {
					CSVByCell[i][j] = Util.RemoveSpaces(CSVByCell[i][j]);
				}
			}
		}

		return CSVByCell;
	}

}
