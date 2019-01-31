using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;

public class CSVC : MonoBehaviour {

	//CSV var needed for info storage before it's outStreamed into a csv;
	private List<String[]> rowInputs;
	private String filePath;

	//CSVConstructor
	public CSVC (string filePath){
		rowInputs = new List<String[]> ();
		this.filePath = filePath;
	}

	/*				.csv addition and creation related functions				*/

	//on the first call creates the csv file, on further calls updates it;
	void fileCA ()
	{
		String[][] output = new String[rowInputs.Count][];
		for (int i = 0; i < output.Length; i++) {
			output [i] = rowInputs [i];
		}
		int length = output.GetLength (0);
		string delim = ",";
		StringBuilder sb = new StringBuilder ();
		for (int i = 0; i < length; i++) {
			sb.AppendLine (string.Join (delim, output [i]));
		}
		StreamWriter outStream = System.IO.File.CreateText (filePath);
		outStream.WriteLine (sb);
		outStream.Close ();
	}

	public double[] paramGen(Day today, Initializer init){
		return new double[] {
			(double) today.get ("day"),
			(double) today.get ("healthyNum"),
			(double) today.get ("healthyWithSympNum"),
			(double) today.get ("sickNum"),
			(double) today.get ("totalSymps"),
			(init.getParam ("percentWithTrtA") * 100),
			(init.getParam ("percentWithTrtB") * 100),
			(double) today.get ("sympTrtA"),
			(double) today.get ("sympTrtB"),
			(double) today.get ("totalSympTrt"),
			(double) today.get ("sickTrtA"),
			(double) today.get ("sickTrtB"),
			(today.get("sickTrtA") * init.getParam("effectiveA")),
			(today.get("sickTrtB") * init.getParam("effectiveB")),
			(double) today.get ("curedANum"),
			(double) today.get ("curedBNum"),
			(double) today.get ("recovered"),
			(double) today.get ("totalCuredDay"),
			(double) today.get ("totalCuredTotal"),
			(double) today.get ("check"),
			(double) today.get ("catchDisease"),
			(double) (today.get ("costNum") - (today.get ("sickNum") * init.getParam("sickCost"))),
			(double) (today.get ("sickNum") * init.getParam("sickCost"))
		};
	}
	//creates and adds the first day and init. params to the .csv;
	public void firstDayAdder (GameBuilder1 world, Initializer init)
	{
		string[] labels = new string[] {
			"Day",
			"# Healthy",
			"# Symptoms",
			"Diseased",
			"Tot Symp",
			"Strategy A",
			"Strategy B",
			"# TRT A",
			"# TRT B",
			"Available to TRT",
			"Disease TRT A #",
			"Disease TRT B #",
			"Rand A",
			"Rand B",
			"TRT A Cures",
			"TRT B Cures",
			"Recover",
			"total cured for day",
			"Sum Disease Cured",
			"Check",
			"Catch Disease",
			"Trt Cost",
			"Sick Cost"
		};
		double[] pars = paramGen(world.retDay(1), init);
		String[] tempInput = new String[23];
		for (int i = 0; i < 23; i++) {
			tempInput [i] = labels [i];
		}
		rowInputs.Add (tempInput);
		tempInput = new String[23];
		for (int i = 0; i < 23; i++) {
			tempInput [i] = "" + pars [i];
		}
		rowInputs.Add (tempInput);
		fileCA ();
	}

	//creates and adds the updated stats lines to the file;
	public void nextDayAdder (GameBuilder1 world, Initializer init)
	{
		double[] pars = paramGen(world.retDay(1), init);
		String[] tempInput = new String[23];
		for (int i = 0; i < 23; i++) {
			tempInput [i] = "" + pars [i];
		}
		rowInputs.Add (tempInput);
		fileCA ();
	}

}
