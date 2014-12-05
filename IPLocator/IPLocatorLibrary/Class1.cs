using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

namespace IPLocatorLibrary
{
    public class SearchLocation
    {
        public string GetLocation(string input)
        {
            try
            {
                Console.WriteLine("[-] 目标：\t" + input);

                if (Regex.IsMatch(input, @"\b(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\b"))
                {
                    IpLocation ipl = IpLocator.GetIpLocation("doloWry.dat", input);
                    return ipl.Country;
                }
                else if (Regex.IsMatch(input, @"([\w-]+\.)*[\w-]+(:\d+)?(/[\u4e00-\u9fa5\w- ./?%&=]*)?$"))
                {
                    Console.WriteLine("[-] 目标为域名，正在判断其IP...");

                    if (input.StartsWith("http://"))
                    {
                        input = input.Replace("http://", "");
                        System.Diagnostics.Debug.WriteLine(input);
                    }

                    IPAddress[] ips;
                    ips = Dns.GetHostAddresses(input);
                    Console.WriteLine("[-] " + input + " 的IP地址为 " + ips[0].ToString());
                    IpLocation ipl = IpLocator.GetIpLocation("doloWry.dat", ips[0].ToString());
                    return ipl.Country;
                }
                else
                {
                    Console.WriteLine("[*] 非法的IP地址或域名格式！");
                    return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("[*] " + ex.Message);
                return null;
            }
        }
    }
}
