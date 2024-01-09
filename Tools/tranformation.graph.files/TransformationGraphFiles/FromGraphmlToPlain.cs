using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace GraphFileTransformations
{
    class FromGraphmlToPlain
    {
        static int pozicija(int[] niz, int vrednost)
        {
            for (int i = 0; i < niz.Length; i++)
                if (niz[i] == vrednost)
                    return (i + 1);
            return -1;
        }

        public static int transform(string putanjaUlaz, string putanjaIzlaz)
        {
            XmlDocument doc = new();
            ISet<CvorGrafa> cvoroviGrafa = new SortedSet<CvorGrafa>();
            int brojCvorova;
            ISet<GranaGrafa> graneGrafa = new SortedSet<GranaGrafa>();
            int brojGrana;
            try
            {
                doc.Load(putanjaUlaz);
                XmlElement? documentElement = doc.DocumentElement;
                XmlNodeList elementi = documentElement!.LastChild!.ChildNodes;
                foreach (XmlNode elemenat in elementi)
                {
                    if (elemenat.Name == "node")
                    {
                        int id = Convert.ToInt32(elemenat!.Attributes!["id"]!.Value);
                        string oznaka = elemenat!.FirstChild!.InnerText;
                        _ = cvoroviGrafa.Add(new CvorGrafa(id, oznaka));
                    }
                }
                brojCvorova = cvoroviGrafa.Count;

                foreach (XmlNode elemenat in elementi)
                {
                    if (elemenat.Name == "edge")
                    {
                        int sourceId = Convert.ToInt32(elemenat!.Attributes!["source"]!.Value);
                        int destinationId = Convert.ToInt32(elemenat!.Attributes!["target"]!.Value);
                        string oznaka = elemenat!.FirstChild!.InnerText;
                        double tezina = Convert.ToDouble(elemenat!.LastChild!.InnerText);
                        _ = graneGrafa.Add(new GranaGrafa(sourceId, destinationId, oznaka, tezina));
                    }
                }
                brojGrana = graneGrafa.Count;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
                return -21;
            }

            int[] mapa = new int[brojCvorova];
            int i = 0;
            foreach (CvorGrafa c in cvoroviGrafa)
                mapa[i++] = c.id;
            try
            {
                using (StreamWriter tokZaUpis = new(putanjaIzlaz))
                {
                    tokZaUpis.WriteLine(" " + brojCvorova + "\t" + brojGrana);
                    foreach (GranaGrafa g in graneGrafa)
                    {
                        tokZaUpis.WriteLine(pozicija(mapa, g.izvorId) + "\t"
                            + pozicija(mapa, g.odredisteId) + "\t"
                            + Convert.ToDouble(g.oznaka)
                            );
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
                return -22;
            }
            return 0;
        }
    }
}
