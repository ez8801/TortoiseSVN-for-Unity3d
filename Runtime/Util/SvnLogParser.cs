using EZ.TortoiseSVN.Data;
using System.Collections.Generic;

namespace EZ.TortoiseSVN.Util
{
    public class SvnLogParser
    {
        const string kRevisionPrefix = "r";

        public IEnumerable<SvnLog> ParseSvnLogData(string orginData)
        {
            char[] separators = new char[] { '\r', '\n' };
            string[] splitByLine = orginData.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
            List<int> startLineIndexs = new List<int>();

            // 시작 인덱스 추출
            for (int i = 0; i < splitByLine.Length - 1; ++i)
            {
                if (splitByLine[i].StartsWith("--") && splitByLine[i + 1].StartsWith(kRevisionPrefix))
                    startLineIndexs.Add(i++);
            }

            for (int i = 0; i < startLineIndexs.Count; ++i)
            {
                int startLineIndex = startLineIndexs[i];
                int endLineIndex = i == startLineIndexs.Count - 1 ? splitByLine.Length - 1 : startLineIndexs[i + 1];

                var log = new SvnLog();
                string[] logInfoRow = splitByLine[startLineIndex + 1].Split('|');

                try
                {
                    log.Revision = int.Parse(logInfoRow[0].Trim().Remove(0, 1)); // "r0302 " > "0302"
                    log.Author = logInfoRow[1].Trim(); // "svn author"

                    var date = logInfoRow[2].Trim();
                    if (System.DateTime.TryParse(date, out var result))
                    {
                        var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
                        log.Date = result.ToString(culture);
                    }
                    else
                        log.Date = date; // 
                }
                catch (System.Exception)
                {
                }

                // affected paths 추출
                string changedPath = string.Empty;
                bool startedChangedPathLine = false;

                for (int j = startLineIndex + 2; j < endLineIndex; ++j)
                {
                    if (splitByLine[j].StartsWith("Changed paths:"))
                    {
                        startedChangedPathLine = true;
                        continue;
                    }

                    if (startedChangedPathLine && splitByLine[j].StartsWith("   "))
                    {
                        // 특정 확장자만 
                        if (splitByLine[j].Contains(".json")
                            || splitByLine[j].Contains(".xlsx"))
                        {
                            changedPath += splitByLine[j] + "\n";
                        }
                        continue;
                    }
                    else
                    {
                        startedChangedPathLine = false;
                    }

                    // log message 추출
                    log.ContextMessage += splitByLine[j] + "\n";
                }

                log.ChangedPath += changedPath;

                yield return log;
            }
        }
    }
}