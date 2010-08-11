using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SongSearch.Web;

namespace Codewerks.SongSearch.Tasks {
	class Program {
		static void Main(string[] args) {

			Console.WriteLine("Welcome to the Task Master");
			Console.WriteLine("What do you want to do?");
			WhatsUp();
		}
		static void WhatsUp() {
			var command = Console.ReadLine();
			RunCommand(command);
		}
		static void RunCommand(string command) {
			var response = "";
			var commands = command != null ? command.Split(' ') : new string[] { };
			var action = commands.First();
			var options = commands.Skip(1);

			switch (action) {
				case "help":
					response = "Help text here";
					break;
				case "exit":
				case "quit":
					Console.WriteLine("bye!");
					Environment.Exit(0);
					break;
				case "minify":
					Console.WriteLine("Squeezing... ");
					//Minifier.SqueezeFiles();
					break;
				case "index":
					Indexer.Index();
					break;

				//case "tags":
				//    Importer.MakeTags();
				//    break;
				case "getid3":
					Importer.GetID3();
					break;
				case "cleanid3":
					Importer.CleanID3();
					break;
				case "texttag":
					Importer.ConvertTextTags();
					break;
				case "uploadnew":
					AmazonAWS.UploadNewFiles();
					break;
				case "uploadall":
					AmazonAWS.UploadAllFiles();
					break;
				case "uploadallsize":
					AmazonAWS.UploadAllFilesWithSize();
					break;
				case "updatemedia":
					AmazonAWS.UpdateContentMedia();
					break;
				
				case "getmissing":
					AmazonAWS.DownloadMissingFiles();
					break;
				default:
					response = "Don't know that one";
					break;
			}
			Console.WriteLine(response);
			WhatsUp();

		}
	}
}
