using System;
/*
  2) g(x)=x^3+x^2+1, d=3, k=4, n=7

  2. Разработать программу, с помощью которой путем имитационного моделирования 
  оценивается вероятность ошибки декодирования при передаче данных по двоично-симметричному каналу. 
  Исходными данными для работы программы являются: порождающий многочлен g (x), 
  длина кодируемой последовательности l (может быть как больше, так и меньше k ) 
  и точность ε, с которой программа оценивает вероятность ошибки декодирования. 
  С помощью программы студент должен исследовать зависимость вероятности ошибки декодирования 
  от значения вероятности появления ошибки в канале при различных значениях l.

  a) Построить график зависимости оценки ошибки декодирования Pe 
  от вероятности ошибки в двоично-симметричном канале p
  при l<k, l>k, l=k. Обосновать полученные зависимости.
 */
namespace Bespr_seti_lab1
{
    class Program
    {
        static int n = 0;

        static int gen_e(int t, double p) //генерация вектора ошибок в(1)
        {

            double m1 = 0;
            int e = 0;
            Random rnd = new Random();

            for (int i = 0; i < t; i++)
            {
                m1 = rnd.NextDouble();
                if (m1 >= p) e += 0;
                else
                {
                    e += 1;

                }
                if (i < t - 1) e = e << 1;
            }

            return e;
        }
        static int gen_m(int t) //генерация m
        {
            double m1 = 0;
            int a = 0, m = 0;
            Random rnd = new Random();
            while (a == 0)
            {
                for (int i = 0; i < t; i++)
                {
                    m1 = rnd.NextDouble();
                    if (m1 >= 0.5) m += 0;
                    else
                    {
                        m += 1;
                        a++;
                    }
                    if (i < t - 1) m = m << 1;
                }
            }
            return m;
        }


        static int delenie(int m, int g, bool s) //основной алгоритм
        {
            int prom_rez = m, d_m, d_g, raz;
            int m_mas = 0;
            int g1 = g;
            d_m = deg(m);
            d_g = deg(g);
            raz = d_m - d_g;
            if (raz < 0)
            {
                return m;
            }
            while (raz >= 0)
            {
                g1 = g;
                g1 = g << raz;
                prom_rez = prom_rez ^ g1;
                d_m = deg(prom_rez);
                raz = d_m - d_g;
            }


            if (s == false) m_mas = m ^ prom_rez;
            else m_mas = prom_rez;
            return m_mas;
        }
        static int deg(int m) //определение макс степень
        {
            int i1 = 0;
            for (int i = 0; i < n; i++)
            {
                if (m % 2 > 0) i1 = i;
                m = m >> 1;
            }
            return i1;
        }

        static void Main(string[] args)
        {
            string otvet = "";
            while (otvet != "q")
            {
                Console.WriteLine("Введите '1' для расчета определенной вероятности и '2' для получения значений для графика");
                otvet = Console.ReadLine();
                double pi = 0, e = 0;

                Console.WriteLine("Введите g(x) в двоичном формате");
                string g_str = Console.ReadLine();
                int g1 = Convert.ToInt32(g_str);

                Console.WriteLine("Введите L");
                n = Convert.ToInt32(Console.ReadLine());
                if (otvet == "1")
                {
                    Console.WriteLine("Введите Pi - вероятность ошибки в бите");
                    pi = Convert.ToDouble(Console.ReadLine());
                }
                Console.WriteLine("Введите e - точность");
                e = Convert.ToDouble(Console.ReadLine());


                int j = 0;
                double pd = 0.9;
                ulong N = (ulong)(0.25 / ((1 - pd) * e * e));
                Console.WriteLine($"N = {N}");



                int d_g = g_str.Length - 1;
                int m = 0;
                int f = 0;
                if (otvet == "2")
                {
                    // Console.WriteLine("Введите m ");
                    // m = Convert.ToInt32(Console.ReadLine());
                    f = 10;
                    pi = 0;
                }
                else m = gen_m(n);
                n = n + d_g;
                int mx = m << d_g;
                int g = 0;
                for (int j1 = 0; j1 < g_str.Length; j1++)
                {
                    g += (g1 % 10) << j1;
                    g1 = g1 / 10;
                }


                for (int k1 = 0; k1 <= f; k1++)
                {

                    for (ulong i = N; i > 0; i--)
                    {
                        int deg_g = d_g;


                        int e_vect = 0;
                        int b = 0;
                        int s = 0;
                        int a = 0;
                        // закомментить если что
                        m = gen_m(n);
                        mx = m << d_g;
                        //
                        a = delenie(mx, g, false);

                        e_vect = gen_e(n, pi);

                        b = a ^ e_vect;

                        s = delenie(b, g, true);
                        if (s == 0)
                        {
                            if (e_vect != 0) j++;

                        }

                    }
                    Console.WriteLine("Ошибка не обнаружена  " + j + "  раз");
                    Console.WriteLine($"Ошибка обнаружена { N - (ulong)j}  раз");

                    Console.WriteLine($" Вероятность ошибки декодирования Pe при P({pi}) =  { (double)((double)j / (double)N)} ");
                    j = 0;
                    if (otvet == "2") pi += 0.1;
                }
            }
        }
    }
}