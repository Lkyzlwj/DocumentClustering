/***********************************************************************

** Author:      Q-WHai
** Create Date: 2016/7/7 11:30:42
** Last Modify: 2016/7/7 11:30:42
** desc：       尚未编写描述
** Ver.:        V0.1.0

************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using org.machine.learning.cluster.kmeans.model;
using org.machine.learning.tfidf;

namespace org.machine.learning.cluster.kmeans.utils
{
    public class KmeansUtils
    {
        public static List<DocumentVector> transformDocuments2Vectors(List<string> documents)
        {
            // TODO
            Dictionary<string, List<string>> wordDict = transformDocuments2Dictionary(documents);
            Dictionary<string, Dictionary<string, double>> tfidfs = new TFIDF(wordDict).execute();

            return transformTFIDFs2Vectors(documents, tfidfs);
        }

        /// <summary>
        /// 以空白字符进行简单分词，并忽略大小写，
        /// 实际情况中可以用其它中文分词算法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<string> filter(string input)
        {
            Regex regex = new Regex("([ \\t{}():;. \n])");
            input = input.ToLower();

            String[] tokens = regex.Split(input);

            List<string> filter = new List<string>();

            for (int i = 0; i < tokens.Length; i++)
            {
                MatchCollection mc = regex.Matches(tokens[i]);
                if (mc.Count <= 0 && tokens[i].Trim().Length > 0
                    && !StopWordsHandler.IsStopword(tokens[i]))
                    filter.Add(tokens[i]);
            }

            return filter;
        }

        private static List<DocumentVector> transformTFIDFs2Vectors(List<string> documents, Dictionary<string, Dictionary<string, double>> tfidfs)
        {
            ISet<string> wordSet = getWordSetFromTFIDFs(tfidfs);
            if (wordSet == null)
            {
                return null;
            }

            List<DocumentVector> vectors = new List<DocumentVector>(tfidfs.Count);
            int segmentIndex = 0;

            foreach (var tfidfItem in tfidfs)
            {
                DocumentVector vector = new DocumentVector();
                foreach (string word in wordSet)
                {
                    if (tfidfItem.Value.ContainsKey(word))
                    {
                        vector.addWeight(tfidfItem.Value[word]);
                    }
                    else
                    {
                        vector.addWeight(0.0);
                    }
                }
                vector.setLabel(documents[segmentIndex]);
                vectors.Add(vector);
                segmentIndex++;
            }

            return vectors;
        }

        private static ISet<string> getWordSetFromTFIDFs (Dictionary<string, Dictionary<string, double>> tfidfs)
        {
            ISet<string> wordSet = new HashSet<string>();
            foreach (var item in tfidfs)
            {
                foreach (var subItem in tfidfs[item.Key])
                {
                    wordSet.Add(subItem.Key);
                }
            }

            return wordSet;
        }

        private static Dictionary<string, List<string>> transformDocuments2Dictionary(List<string> documents)
        {
            Dictionary<string, List<string>> wordDict = new Dictionary<string, List<string>>();
            List<string> wordList = null;
            for (int index = 0; index < documents.Count; index++)
            {
                List<string> documentWords = filter(documents[index]); // documents[index].Split(new Char[] { ' ' });
                wordList = new List<string>();
                foreach (string word in documentWords)
                {
                    wordList.Add(word);
                }

                wordDict.Add(index + "", wordList);
            }

            return wordDict;
        }
    }
}
