using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBrotherViewer.Model
{
    public static class AttentionsOperation
    {
        private const string path = "attentions.txt";
        public static void SaveAttentionsToTextFile(IEnumerable<Attention> attentions)
        {
            using (var sw = new StreamWriter(File.Create(path)))
            {
                if (attentions == null) return;
                foreach (var attention in attentions)
                {
                    sw.WriteLine(attention.Name);
                }
            }
        }

        public static List<Attention> LoadAttentionsToTextFile()
        {
            if (!File.Exists(path))
            {
                SaveAttentionsToTextFile(null);
            }

            var attentionsList = new List<Attention>();
            using (var sr = new StreamReader(File.OpenRead(path)))
            {
                while (!sr.EndOfStream)
                {
                    attentionsList.Add(new Attention(){Name = sr.ReadLine()});
                }
            }
            return attentionsList;
        }

    }
}
