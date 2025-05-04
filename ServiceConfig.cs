using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryCleanerService
{
	public class ServiceConfig
	{
		public List<string> FoldersToClean { get; set; } = new();
		public int DeleteFilesOlderThanDays { get; set; } = 7;
		public int CheckIntervalMinutes { get; set; } = 120;

	}
}
