using System.Collections.Generic;
using System.Collections;
using System;

namespace Data
{
    public class DataProcess
    {
        //16个类型的包的队列（16个队列）
        public static Queue<ArrayList> queue_01H = new Queue<ArrayList>();  //01H包的队列
        public static Queue<ArrayList> queue_02H = new Queue<ArrayList>();  //02H包的队列
        public static Queue<ArrayList> queue_04H = new Queue<ArrayList>();  //04H包的队列
        public static Queue<ArrayList> queue_05H = new Queue<ArrayList>();  //05H包的队列
        public static Queue<ArrayList> queue_06H = new Queue<ArrayList>();  //06H包的队列
        public static Queue<ArrayList> queue_07H = new Queue<ArrayList>();  //07H包的队列
        public static Queue<ArrayList> queue_08H = new Queue<ArrayList>();  //08H包的队列
        public static Queue<ArrayList> queue_09H = new Queue<ArrayList>();  //09H包的队列
        public static Queue<ArrayList> queue_0AH = new Queue<ArrayList>();  //0AH包的队列
        public static Queue<ArrayList> queue_0BH = new Queue<ArrayList>();  //0BH包的队列
        public static Queue<ArrayList> queue_0CH = new Queue<ArrayList>();  //0CH包的队列
        public static Queue<ArrayList> queue_0DH = new Queue<ArrayList>();  //0DH包的队列
        public static Queue<ArrayList> queue_0EH = new Queue<ArrayList>();  //0EH包的队列
        public static Queue<ArrayList> queue_0FH = new Queue<ArrayList>();  //0FH包的队列
        public static Queue<ArrayList> queue_10H = new Queue<ArrayList>();  //10H包的队列
        public static Queue<ArrayList> queue_11H = new Queue<ArrayList>();  //11H包的队列

        //12,13不知道?
        //Key->标识符的十进制,Value->包长度
        //一共16种类型的包
        private static Dictionary<int, int> LenMap = new Dictionary<int, int>{
                {1,7},{2,6},{4,6},{5,6},{6,4},{7,8},{8,9},{9,3},{10,9},{11,4},{12,3},{13,3},{14,10},{15,3},{16,5},{17,7}
            };

        //将原始数据转换为真实的需要的数据
        public static void RawToRealData(int[] data_int)
        {
            List<ArrayList> ls = new List<ArrayList>();  //保存读取出来的一系列的包的列表
            ArrayList t = new ArrayList();  //临时变量，用来根据标识符01H、02H等找出来每一个包中的内容
            for (int i = 0; i < data_int.Length; i++)
            {
                //如果是标识字节（数据字节最高位一定为0，所以一定<128，&128一定为0）
                if ((data_int[i] & (1 << 7)) == 0)
                {
                    System.Console.WriteLine("Tag-" + data_int[i]); //打印出来表示位
                    t.Add(data_int[i]);  //往包中加进这个元素（包的标识符）
                    for (int j = i + 1; j <= data_int.Length; j++)
                    {
                        if (j == data_int.Length)  //如果是最后一个元素，那么
                        {
                            ArrayList temp = t.Clone() as ArrayList;
                            ls.Add(temp);
                            t.Clear();  //在将包加入列表之后，将临时包的内容清空
                            break;  //结束循环，不然会执行后面的循环，导致OutOfIndex
                        }
                        //如果是数据字节（数据字节最高位一定为1，所以一定＞=128），加进列表中
                        if (data_int[j] >= 128)
                        {
                            t.Add(data_int[j]);
                        }
                        else
                        { //如果是下一个标志位，那么将当前临时包的内容加进列表并且移动指针i到下一个标识位
                            ArrayList temp = t.Clone() as ArrayList;
                            ls.Add(temp);
                            t.Clear();  //清空
                            i = j - 1;  //i的值换为j的值，下标前进，但是由于执行完i还要++，因此i=j-1
                            break;  //结束循环
                        }
                    }
                }
            }

            List<ArrayList> ls_real_data = new List<ArrayList>();
            //遍历每个包
            for (int j = 0; j < ls.Count; j++)
            {
                int key = (int)ls[j][0];
                //如果包的长度和字典当中预定义的长度不一致
                //打印包丢失情况的相关信息
                if (ls[j].Count != LenMap[key])
                {
                    System.Console.WriteLine("标识为" + key + "的包丢失");
                }
                else  //当包是正确的
                {
                    ArrayList _arr = new ArrayList();
                    for (int i = 0; i < ls[j].Count; i++)
                    {
                        if (i < 2)
                        {
                            _arr.Add(ls[j][i]);
                            continue;
                        }
                        //如果对应的位为1，那么数据不发生改变
                        if (((int)ls[j][1] & (1 << i)) == 1)
                        {
                            _arr.Add(ls[j][i]);
                        }
                        else  //否则，去掉Bit7
                        {
                            //下面两种方式等价
                            _arr.Add((int)ls[j][i] - 128);
                        }
                    }
                    ls_real_data.Add(_arr);
                }
            }

            for (int i = 0; i < ls_real_data.Count; i++)
            {

                switch ((int)ls_real_data[i][0])  //根据类型的标识符判断入哪个队列
                {
                    case 1: queue_01H.Enqueue(ls_real_data[i]); break;
                    case 2: queue_02H.Enqueue(ls_real_data[i]); break;
                    case 4: queue_04H.Enqueue(ls_real_data[i]); break;
                    case 5: queue_05H.Enqueue(ls_real_data[i]); break;
                    case 6: queue_06H.Enqueue(ls_real_data[i]); break;
                    case 7: queue_07H.Enqueue(ls_real_data[i]); break;
                    case 8: queue_08H.Enqueue(ls_real_data[i]); break;
                    case 9: queue_09H.Enqueue(ls_real_data[i]); break;
                    case 10: queue_0AH.Enqueue(ls_real_data[i]); break;
                    case 11: queue_0BH.Enqueue(ls_real_data[i]); break;
                    case 12: queue_0CH.Enqueue(ls_real_data[i]); break;
                    case 13: queue_0DH.Enqueue(ls_real_data[i]); break;
                    case 14: queue_0EH.Enqueue(ls_real_data[i]); break;
                    case 15: queue_0FH.Enqueue(ls_real_data[i]); break;
                    case 16: queue_10H.Enqueue(ls_real_data[i]); break;
                    case 17: queue_11H.Enqueue(ls_real_data[i]); break;
                    default: break;
                }

                for (int j = 0; j < ls_real_data[i].Count; j++)
                {
                    System.Console.Write(ls_real_data[i][j] + " ");
                }
                System.Console.WriteLine();
            }

        }
        //提供一个重载，有可能要传入的参数是字符串
        public static void RawToRealData(String data1)
        {
            String[] vs = data1.Split(" ");  //空格分隔
            int[] data_int = new int[vs.Length];  //给数组分配空间，保存int类型数据
            for (int i = 0; i < vs.Length; i++) //转换十六进制->十进制
            {
                data_int[i] = Convert.ToInt32(vs[i], 16);
            }
            RawToRealData(data_int);  //调用int的方法
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            String data = "90 FF FF FF 80 8B 02 80 FF FF FF 80 02 80 FF FF FF 80 02 80 FF FF FF 80 01 90 FF FF FF 80 8C 02 80 FF FF FF 80 02 80 FF FF FF 80 02 80 FF FF FF 80 01 90 FF FF FF 80 8D 02 80 FF FF FF 80 02 80 FF FF FF 80 02 80 FF FF FF 80 01 90 FF FF FF 80 8E 02 82 FF 80 FF 80 02 82 FF 80 FF 80 02 82 FF 80 FF 80 01 92 FF 80 FF 80 8F 02 82 FF 80 FF 80 02 82 FF 80 FF 80 02 82 FF 80 FF 80 01 92 FF 80 FF 80 91 02 82 FF 80 FF 80 02 82 FF 80 FF 80 02 82 FF 80 FF 80 01 92 FF 80 FF 80 92 02 82 FF 80 FF 80 02 82 FF 80 FF 80 02 82 FF 80 FF 80 01 90 FF FF FF 80 93 02 80 FF FF FF 80 02 80 FF FF FF 80 02 83 80 80 FF 80 01 97 80 80 80 80 94 02 87 80 80 80 80 02 87 80 80 80 80 02 87 81 80 81 80 01 97 82 81 82 80 95 02 87 83 81 83 80 02 87 83 82 84 80 02 87 84 82 84 80 01 97 85 83 85 80 96 02 87 85 83 85 80 02 87 85 83 85 80 02 87 85 83 86 80 01 97 86 84 86 80 98 02 87 86 84 86 80 02 87 86 84 86 80 02 87 86 84 87 80 01 97 86 84 87 80 99 02 87 87 84 87 80 02 87 86 84 87 80 02 87 86 84 87 80 01 97 86 84 86 80 9A 02 87 86 84 86 80 02 87 85 83 86 80 02 87 85 83 85 80 01 97 84 83 85 80 9B 02 87 84 82 84 80 02 87 83 82 84 80 02 87 83 82 83 80 01 97 82 81 83 80 9C 02 87 82 81 82 80 02 87 81 80 82 80 02 87 80 80 81 80 01 97 80 80 80 80 9D 02 87 80 80 80 80 02 82 FF 80 FF 80 02 80 FF FF FF 80 01 90 FE FF FE 80 9E 02 80 FE FF FE 80 02 80 FF FF FE 80 02 80 FF FF FE 80 01 90 FF FF FE 80 A0 02 80 FF FF FE 80 02 80 FF FF FE 80 02 80 FF FF FE 80 01 90 FF FF FF 80 A1 02 80 FF FF FF 80 02 80 FF FF FF 80 02 80 FF FF FF 80 01 90 FF FF FF 80 A2 02 80 FF FF FF 80 02 80 FF FF FF 80 02 80 FF FF FE 80 01 90 FF FF FE 80 A3 02 80 FF FF FE 80 02 80 FF FF FE 80 02 80 FF FF FF 80 01 92 FF 80 FF 80 A4 02 82 FF 80 FF 80 02 80 FF FF FF 80 02 80 FF FF FF 80 01 90 FF FF FF 80 A5 02 80 FF FF FF 80 02 80 FF FF FF 80 02 80 FF FF FE 80 01 90 FF FF FE 80 A6 02 83 80 80 FF 80 02 83 80 80 FF 80 02 83 80 80 FF 80 01 90 FF FF FF 80 A7 02 80 FF FF FF 80 02 80 FF FF FF 80 02 80 FF FF FF 80 01 90 FF FF FF 80 A8 02 80 FF FF FF 80 02 80 FF FF FF 80 02 80 FF FF FF 80 01 90 FF FF FF 80 AA 02 80 FE FF FF 80 02 80 FE FE FE 80 02 80 FD FE FE 80 01 90 FB FD FE 80 AB 02 80 FA FC FE 80 02 80 F8 FA FD 80 02 80 F7 FA FD 80 01 90 F8 FA FE 80 AC 02 80 FA FC FF 80 02 84 FE FF 80 80 02 87 83 82 83 80 01 97 8B 88 85 80 AD 02 87 95 8E 88 80 02 87 9F 96 8C 80 02 87 AA 9D 90 80 01 97 B4 A4 93 80 AE 02 87 BE AB 95 80 02 87 C5 B0 97 80 02 87 CA B4 98 80 01 97 CA B4 97 80 AF 02 87 C7 B2 94 80 02 87 C2 AE 90 80 02 87 BA A9 8A 80 01 97 B0 A2 83 80 B0 02 83 A6 9B FC 80 02 83 9A 93 F3 80 02 83 8F 8A E9 80 01 93 84 83 DF 80 B1 02 80 FD FE D6 80 02 80 F7 FA D2 80 02 80 F4 F8 D2 81 01 90 F3 F7 D7 80 B2 02 80 F3 F7 DD 80 02 80 F4 F8 E5 80 02 80 F6 F9 ED 80 01 90 F7 FA F4 80 B3 02 80 F9 FB F9 80 02 80 FA FC FE 80 02 84 FB FD 80 80 01 94 FC FD 81 80 B4 02 84 FC FD 81 80 02 84 FC FD 81 80 02 84 FC FD 80 80 01 90 FC FD FF 80 B5 02 80 FB FD FF 80 02 80 FB FD FF 80 02 80 FB FD FF 80 01 94 FC FD 80 80 B6 02 84 FC FD 80 80 02 84 FC FD 80 80 02 84 FC FD 80 80 01 94 FC FD 80 80 B7 02 84 FC FD 80 80 02 80 FC FD FF 80 02 80 FC FD FF 80 01 90 FC FD FF 80 B8 02 80 FC FD FF 80 02 84 FC FD 80 80 02 84 FC FD 80 80 01 94 FD FE 80 80 B9 02 84 FD FE 80 80 02 84 FD FE 80 80 02 84 FC FD 80 80 01 90 FC FD FF 80 BA 02 80 FC FD FF 80 02 80 FC FD FF 80 02 80 FD FD FF 80 01 94 FD FE 80 80 BB 02 84 FD FE 80 80 02 84 FD FE 80 80 02 84 FD FE 80 80 01 94 FD FE 80 80 BC 02 84 FC FD 80 80 02 84 FC FD 80 80 02 84 FC FD 80 80 01 94 FC FE 80 80 BD 02 84 FC FE 80 80 02 84 FD FE 80 80 02 84 FD FE 80 80 01 94 FD FE 80 80 BE 02 84 FD FE 80 80 02 84 FD FE 80 80 02 84 FD FE 80 80 01 94 FD FE 80 80 BF 02 84 FD FE 80 80 02 84 FD FE 80 80 02 84 FD FE 80 80 01 94 FD FE 80 80 C0 02 84 FD FE 80 80 02 84 FD FE 80 80 02 84 FD FE 80 80 09 80 81 11 80 83 A6 82 A6 82 01 94 FD FE 80 80 C1 0E 02 84 FD FE 80 80 02 84 FD FE 80 80 02 84 FD FE 80 80 01 94 FD FE 80 80 C2 02 84 FD FE 80 80 02 84 FD FE 80 80 02 84 FD FE 80 80 01 94 FD FE 80 80 C3 02 84 FD FE 80 80 02 84 FD FE 80 80 02 84 FD FE 80 80 01 94 FE FE 80 80 C4 02 84 FE FF 80 80 02 84 FF FF 81 80 0F 80 80 04 80 BC 80 8F 80 0B 80 80 80 05 80 96 A7 81 06 80 80 80 07 8F F6 FF F8 FF 80 80 10 80 80 A7 81 02 86 FF 80 81 80 01 97 80 80 82 80 C5 02 87 80 80 84 80 02 87 81 80 84 80 02 87 82 81 85 80 01 97 82 81 86 80 C6 02 87 83 82 87 80 02 87 84 82 87 80 02 87 85 83 88 80 01 97 85 83 88 80 C6 02 87 86 84 89 80 02 87 86 84 89 80 02 87 87 85 8A 80 01 97 88 85 8B 80 C7 02 87 89 86 8C 80 02 87 89 86 8C 80 02 87 8A 86 8D 80 01 97 8A 87 8D 80 C8 02 87 8A 87 8D 80 02 87 8B 87 8E 80 02 87 8B 87 8E 80 01 97 8C 88 8F 80 C9 02 87 8C 88 8F 80 02 87 8D 88 8F 80 02 87 8D 89 90 80 01 97 8D 89 90 80 CA 02 87 8D 89 90 80 02 87 8E 89 90 80 02 87 8E 89 90 80 01 97 8D 89 90 80 CB 02 87 8D 89 90 80 02 87 8D 89 90 80 02 87 8D 89 90 80 01 97 8D 89 90 80 CC 02 87 8D 89 90 80 02 87 8D 89 90 80 02 87 8D 89 90 80 01 97 8D 89 90 80 CD 02 87 8D 89 8F";
            DataProcess.RawToRealData(data);
            System.Console.WriteLine("01H包数量为" + DataProcess.queue_01H.Count);
            System.Console.WriteLine("02H包数量为" + DataProcess.queue_02H.Count);
            System.Console.WriteLine("04H包数量为" + DataProcess.queue_04H.Count);
            System.Console.WriteLine("05H包数量为" + DataProcess.queue_05H.Count);
            System.Console.WriteLine("06H包数量为" + DataProcess.queue_06H.Count);
            System.Console.WriteLine("07H包数量为" + DataProcess.queue_07H.Count);
            System.Console.WriteLine("08H包数量为" + DataProcess.queue_08H.Count);
            System.Console.WriteLine("09H包数量为" + DataProcess.queue_09H.Count);
            System.Console.WriteLine("0AH包数量为" + DataProcess.queue_0AH.Count);
            System.Console.WriteLine("0BH包数量为" + DataProcess.queue_0BH.Count);
            System.Console.WriteLine("0CH包数量为" + DataProcess.queue_0CH.Count);
            System.Console.WriteLine("0DH包数量为" + DataProcess.queue_0DH.Count);
            System.Console.WriteLine("0EH包数量为" + DataProcess.queue_0EH.Count);
            System.Console.WriteLine("0FH包数量为" + DataProcess.queue_0FH.Count);
            System.Console.WriteLine("10H包数量为" + DataProcess.queue_10H.Count);
            System.Console.WriteLine("11H包数量为" + DataProcess.queue_11H.Count);
            ArrayList arrayList = DataProcess.queue_01H.Dequeue();
            foreach (var item in arrayList)
            {
                System.Console.Write(item + " ");
            }
            System.Console.WriteLine("\n01H包数量为" + DataProcess.queue_01H.Count);
        }
    }
}
