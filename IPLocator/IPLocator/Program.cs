using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

namespace IPLocator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string tar = args[0];
                Console.WriteLine("[-] 目标：\t" + tar);
                if (Regex.IsMatch(tar, @"\b(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\b"))
                {
                    IpLocation ipl = IpLocator.GetIpLocation("doloWry.dat", tar);
                    //Console.WriteLine("[+] IP所在地: " + ipl.Country + " " + ipl.City, "地址段:" + IpLocator.IntToIpString(ipl.IpStart) + "-" + IpLocator.IntToIpString(ipl.IpEnd));
                    Console.WriteLine("[+] 地区：\t" + ipl.Country);
                    Console.WriteLine("[+] 运营商：\t" + ipl.City);
                    Console.WriteLine("[+] 地址段：\t" + IpLocator.IntToIpString(ipl.IpStart) + "-" + IpLocator.IntToIpString(ipl.IpEnd));
                }
                else if (Regex.IsMatch(tar, @"([\w-]+\.)*[\w-]+(:\d+)?(/[\u4e00-\u9fa5\w- ./?%&=]*)?$"))
                {
                    Console.WriteLine("[-] 目标为域名，正在判断其IP...");
                    
                    if (tar.StartsWith("http://"))
                    {
                        tar = tar.Replace("http://", "");
                        System.Diagnostics.Debug.WriteLine(tar);
                    }
                    
                    IPAddress[] ips;
                    ips = Dns.GetHostAddresses(tar);
                    Console.WriteLine("[-] " + tar + " 的IP地址为 " + ips[0].ToString());
                    IpLocation ipl = IpLocator.GetIpLocation("doloWry.dat", ips[0].ToString());
                    Console.WriteLine("[+] 地区：\t" + ipl.Country);
                    Console.WriteLine("[+] 运营商：\t" + ipl.City);
                    Console.WriteLine("[+] 地址段：\t" + IpLocator.IntToIpString(ipl.IpStart) + "-" + IpLocator.IntToIpString(ipl.IpEnd));
                }
                else
                {
                    Console.WriteLine("[*] 非法的IP地址或域名格式！");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("[*] "+ex.Message);
            }
            Console.ReadKey();
        }
    }
}
