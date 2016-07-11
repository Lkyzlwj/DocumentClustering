/***********************************************************************

** Author:      Q-WHai
** Create Date: 2016/7/7 10:16:14
** Last Modify: 2016/7/7 10:16:14
** desc：       尚未编写描述
** Ver.:        V0.1.0

************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using org.common.utils.file;
using org.machine.learning.tfidf;

namespace TestDemos
{
    class TestTFIDF
    {
        private string dataFolderFullName = String.Concat(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\Data\TF-IDF");
        private Dictionary<String, List<String>> allDocumentWords = new Dictionary<String, List<String>>();

        public TestTFIDF()
        {
            readAllWords();

            TFIDF tfidf = new TFIDF(allDocumentWords);
            tfidf.execute();
        }

        private void readAllWords()
        {
            FileInfo[] fileInfos = FileUtils.getAllSubFile(dataFolderFullName);
            foreach (FileInfo fileInfo in fileInfos)
            {
                List<string> fileWords = FileUtils.read(fileInfo.FullName);
                allDocumentWords.Add(fileInfo.Name, fileWords);
            }
        }
    }
}
