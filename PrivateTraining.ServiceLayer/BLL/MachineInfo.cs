using System;
using System.Collections;
using System.Management;
using System.Management.Instrumentation;

namespace PrivateTraining.ServiceLayer.BLL
{

    class HardDrive
    {
        private string model = null;
        private string type = null;
        private string serialNo = null;
        public string Model
        {
            get { return model; }
            set { model = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string SerialNo
        {
            get { return serialNo; }
            set { serialNo = value; }
        }
    }

    public class MachineInfo
    {

        public string GetInfo()
        {
            ArrayList hdCollection = new ArrayList();

            ManagementObjectSearcher searcher = new
                ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                HardDrive hd = new HardDrive();
                hd.Model = wmi_HD["Model"].ToString();
                hd.Type = wmi_HD["InterfaceType"].ToString();

                hdCollection.Add(hd);
            }

            searcher = new
                ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");

            int i = 0;
            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                // get the hard drive from collection
                // using index
                HardDrive hd = (HardDrive)hdCollection[i];

                // get the hardware serial no.
                if (wmi_HD["SerialNumber"] == null)
                    hd.SerialNo = "None";
                else
                    hd.SerialNo = wmi_HD["SerialNumber"].ToString();

                ++i;
            }
            string Data = "";
            // Display available hard drives
            foreach (HardDrive hd in hdCollection)
            {
                Data = "Model\t\t: " + hd.Model;
                Data += "Model\t\t: " + hd.Type;
                Data += "Model\t\t: " + hd.SerialNo;
                //Console.WriteLine("Model\t\t: " + hd.Model);
                //Console.WriteLine("Type\t\t: " + hd.Type);
                //Console.WriteLine("Serial No.\t: " + hd.SerialNo);
                //Console.WriteLine();
            }
            return Data;
            // Pause application
            //Console.WriteLine("Press [Enter] to exit...");
            //Console.ReadLine();
        }
        public string GetInfoSerialNumber()
        {
            ArrayList hdCollection = new ArrayList();

            ManagementObjectSearcher searcher = new
                ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                HardDrive hd = new HardDrive();
                hd.Model = wmi_HD["Model"].ToString();
                hd.Type = wmi_HD["InterfaceType"].ToString();

                hdCollection.Add(hd);
            }

            searcher = new
                ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");

            int i = 0;
            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                // get the hard drive from collection
                // using index
                HardDrive hd = (HardDrive)hdCollection[i];

                // get the hardware serial no.
                if (wmi_HD["SerialNumber"] == null)
                    hd.SerialNo = "None";
                else
                    hd.SerialNo = wmi_HD["SerialNumber"].ToString();

                ++i;
            }
            string Data = "";
            // Display available hard drives
            foreach (HardDrive hd in hdCollection)
            {
                //Data = "Model\t\t: " + hd.Model;
                //Data += "Model\t\t: " + hd.Type;
                Data = hd.SerialNo;
                //Console.WriteLine("Model\t\t: " + hd.Model);
                //Console.WriteLine("Type\t\t: " + hd.Type);
                //Console.WriteLine("Serial No.\t: " + hd.SerialNo);
                //Console.WriteLine();
            }
            return Data;
            // Pause application
            //Console.WriteLine("Press [Enter] to exit...");
            //Console.ReadLine();
        }

    }
}
