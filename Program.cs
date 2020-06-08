using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuvarCSHARP
{
    class Fuvar
    {
        public int taxiId { get; set; }
        public DateTime indulas { get; set; }
        public int idotartam { get; set; }
        public double tavolsag { get; set; }
        public double viteldij { get; set; }
        public double borravalo { get; set; }
        public string fizetes { get; set; }

        public Fuvar(int taxiId, DateTime indulas, int idotartam, double tavolsag, double viteldij, double borravalo, string fizetes)
        {
            this.taxiId = taxiId;
            this.indulas = indulas;
            this.idotartam = idotartam;
            this.tavolsag = tavolsag;
            this.viteldij = viteldij;
            this.borravalo = borravalo;
            this.fizetes = fizetes;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            #region 2. feladat
            List<Fuvar> beolvasas()
            {
                List<Fuvar> lista = new List<Fuvar>();
                try
                {
                    using (StreamReader sr=new StreamReader(new FileStream("fuvar.csv",FileMode.Open),Encoding.UTF8))
                    {
                        sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            var split = sr.ReadLine().Split(';');
                            var o = new Fuvar(
                                    Convert.ToInt32(split[0]),
                                    Convert.ToDateTime(split[1]),
                                    Convert.ToInt32(split[2]),
                                    Convert.ToDouble(split[3]),
                                    Convert.ToDouble(split[4]),
                                    Convert.ToDouble(split[5]),
                                    split[6]
                                );
                            lista.Add(o);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Hiba a bolvasásnál. "+e.Message);
                }
                return lista;
            }
            List<Fuvar> fuvarLista = beolvasas();
            #endregion
            #region 3. feladat
            Console.WriteLine($"3. feladat: {fuvarLista.Count()} fuvar");
            #endregion

            #region 4. feladat

            var fuv = fuvarLista.Where(x => x.taxiId == 6185).ToList();
            Console.WriteLine($"4. feladat: {fuv.Count()} fuvar alatt: {fuv.Sum(x=>x.viteldij)}$");
            #endregion

            #region 5. feladat
            Console.WriteLine("5. feladat: ");
            var groupFuv=fuvarLista.GroupBy(x => x.fizetes).Select(
                y => new
                {
                    fizetes=y.Key,
                    db=y.Count()
                }
            ).ToList() ;
            groupFuv.ForEach(x => 
                    Console.WriteLine("\t"+x.fizetes+": "+x.db+" fuvar")
            );
            #endregion

            #region 6. feladat
            var merfold = fuvarLista.Sum(x => x.tavolsag);
            Console.WriteLine("6. feladat: {0}km",Math.Round(merfold*1.6,2));
            #endregion

            #region 7. feladat
            Console.WriteLine("7. feladat: Leghosszabb fuvar: ");
            var legh = fuvarLista.OrderByDescending(x=>x.idotartam).First();
            Console.WriteLine($"\tFuvar hossza: {legh.idotartam} másodperc");
            Console.WriteLine($"\tTaxi azonosító: {legh.taxiId}");
            Console.WriteLine($"\tMegtett távolság: {legh.tavolsag*1.6} km");
            Console.WriteLine($"\tViteldíj: {legh.viteldij}$");
            #endregion

            #region 8. feladat
            var hibasLista = fuvarLista.OrderBy(x=>x.indulas).Where(x =>
                  x.idotartam > 0 &&
                  x.viteldij > 0 &&
                  x.tavolsag == 0).ToList();
            using (StreamWriter sw=new StreamWriter(new FileStream("hibak.txt",FileMode.Create),Encoding.UTF8))
            {
                sw.WriteLine("taxi_id;indulas;idotartam;tavolsag;viteldij;borravalo;fizetes_modja");
                hibasLista.ForEach(x=>
                        sw.WriteLine(
                            x.taxiId+";"+
                            x.indulas.ToString("yyyy-MM-dd hh:mm:ss")+";"+
                            x.idotartam+";"+
                            x.tavolsag+";"+
                            x.viteldij+";"+
                            x.borravalo+";"+
                            x.fizetes)
                        );
            }
                Console.WriteLine("8. feladat: hibak.txt");
            #endregion


            Console.ReadKey();
        }
    }
}
