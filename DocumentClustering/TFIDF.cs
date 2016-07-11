/***********************************************************************

** Author:      Q-WHai
** Create Date: 2016/7/7 9:52:42
** Last Modify: 2016/7/7
** desc：       计算 TF-IDF 的核心类
** Ver.:        V0.1.0

************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using org.common.utils;

namespace org.machine.learning.tfidf
{
    public class TFIDF
    {
        private Dictionary<String, List<String>> allDocumentWords = null;

        public TFIDF(Dictionary<String, List<String>> allDocumentWords)
        {
            this.allDocumentWords = allDocumentWords;
        }

        public Dictionary<string, Dictionary<string, double>> execute()
        {
            // 所有文件中单词的 TF 值 
            Dictionary<string, Dictionary<string, double>> allTFs = getAllTFs(allDocumentWords);

            // 所有单词的 IDF 值
            Dictionary<string, double> allIDFs = statisticsIDFs(allDocumentWords);

            // TODO TF-IDF
            Dictionary<string, Dictionary<string, double>> allTFIDFs = statisticsTFIDF(allTFs, allIDFs);

            return allTFIDFs;
        }

        // ------------------------------------------- tf --------------------------------------------

        private Dictionary<string, Dictionary<string, double>> getAllTFs(Dictionary<string, List<string>> data)
        {
            Dictionary<string, Dictionary<string, double>> result = new Dictionary<string, Dictionary<string, double>>(); // 所有文件中单词的 TF 值

            foreach (var item in data)
            {
                result.Add(item.Key, statisticsTF(item.Value));
            }
        
            return result;
        }

        private Dictionary<string, double> statisticsTF(List<string> words)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();
        
            int totalCount = 0;
            Dictionary<string, int> wordCount = new Dictionary<string, int>();
        
            // word count
            foreach (string word in words)
            {
                totalCount++;
                if (wordCount.ContainsKey(word))
                {
                    wordCount[word] += 1;
                }
                else
                {
                    wordCount.Add(word, 1);
                }
            }

            int maxCount = 0;
            foreach (var word in wordCount)
            {
                if (maxCount < word.Value)
                {
                    maxCount = word.Value;
                }
            }
        
            // TFs
            foreach(var item in wordCount)
            {
                result.Add(item.Key, 1.0 * item.Value / maxCount);
            }
            
            return result;
        }

        // ------------------------------------------- idf -------------------------------------------
        private Dictionary<string, double> statisticsIDFs (Dictionary<string, List<string>> data)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();
            int totalFileCount = data.Count;
        
            // 统计所有单词被包含在了多个文件中
            Dictionary<string, int> allWordsFreq = new Dictionary<string, int>();
            ISet<string> fileWords = null;
            
            foreach (var item in data)
            {
                fileWords = new HashSet<string>();
                foreach (string word in item.Value)
                {
                    fileWords.Add(word);
                }

                foreach (string word in fileWords)
                {
                    if (allWordsFreq.ContainsKey(word))
                    {
                        allWordsFreq[word] += 1;
                    }
                    else
                    {
                        allWordsFreq.Add(word, 1);
                    }
                }

                fileWords.Clear();
            }
            
            // 统计所有单词的 IDF
            foreach (var item in allWordsFreq)
            {
                result.Add(item.Key, CommonUtils.log2(1.0 * totalFileCount / (item.Value + 1)));
            }
            
            return result;
        }

        // ------------------------------------------- tf-idf -------------------------------------------
        private Dictionary<string, Dictionary<string, double>> statisticsTFIDF(Dictionary<string, Dictionary<string, double>> allTFs, Dictionary<string, double> allIDFs) {
            Dictionary<string, Dictionary<string, double>> result = new Dictionary<string, Dictionary<string, double>>();
            
            Dictionary<string, double> oneFileTFs = null;
            foreach (var tfItem in allTFs)
            {
                oneFileTFs = allTFs[tfItem.Key];
                foreach (var oneFileItem in oneFileTFs)
                {
                    double tfidf = oneFileTFs[oneFileItem.Key] * allIDFs[oneFileItem.Key];
                    if (result.ContainsKey(tfItem.Key))
                    {
                        Dictionary<string, double> singleWordTfidf = result[tfItem.Key];
                        singleWordTfidf.Add(oneFileItem.Key, tfidf);
                    }
                    else
                    {
                        Dictionary<string, double> singleWordTfidf = new Dictionary<string, double>();
                        singleWordTfidf.Add(oneFileItem.Key, tfidf);
                        result.Add(tfItem.Key, singleWordTfidf);
                    }
                }
            }
            
            return result;
        }
    }
}
