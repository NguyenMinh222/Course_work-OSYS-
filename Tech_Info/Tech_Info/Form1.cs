using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Diagnostics;

namespace Tech_Info
{
    public partial class Form1 : Form
    {
        PerformanceCounter CPU = new PerformanceCounter("Processor","% Processor Time","_Total");
        PerformanceCounter RAM = new PerformanceCounter("Memory","Available MBytes");
        PerformanceCounter SYS = new PerformanceCounter("System","System Up Time");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();

            //С помощью MachineName получаем имя компьютера
            String nameOfComputer = Environment.MachineName;
            NameOfComputerTextBox.Text = nameOfComputer;

            //версия компьютера
            String versionOfComputer = Environment.OSVersion.Version.ToString();
            VersionTextBox.Text = versionOfComputer;

            //Версия операционной системы
            OSTextBox.Text = Environment.OSVersion.ToString();

            //Платформа ОС
            OSPTextBox.Text = Environment.OSVersion.Platform.ToString();

            //Имя пользователя
            UserNameTextBox.Text = Environment.UserName;

            //Название процессора
            NameOfProccesor.Text = GetComponent("Win32_Processor","Name");

            CodeNameOfProcessor.Text = GetComponent("Win32_Processor", "Manufacturer");
           
           // GetHardWareInfo("Win32_VideoController", listView1);

            //Stepping
            string word = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER");
            string[] word_s = word.Split(' ');
            for (int i = 0; i < word_s.Length; i++)
            {
                if (word_s[i] == "Family")
                {
                    FamilyTextBox.Text = word_s[i + 1];
                }
                if (word_s[i] == "Model")
                {
                    ModelTextBox.Text = word_s[i+1];
                }
                if (word_s[i] == "Stepping")
                {
                    string strRemove = word_s[i + 1];
                    int indexRemove = strRemove.Length - 1;
                    strRemove = strRemove.Remove(indexRemove);
                    SteppingTextBox.Text = strRemove;
                }    
            }

            //Revision
            RevisionTextBox.Text = GetComponent("Win32_Processor", "Revision");

            //Caches
            L2Size.Text = GetComponent("Win32_Processor", "L2CacheSize")+" KBytes";
            L3Size.Text = GetComponent("Win32_Processor", "L3CacheSize") + " KBytes";
            L3Speed.Text = GetComponent("Win32_Processor", "L3CacheSpeed");

            //Voltage
            VoltageTextBox.Text = GetComponent("Win32_Processor", "CurrentVoltage") + " W";

            //Clock speed
            Clock.Text = GetComponent("Win32_Processor", "MaxClockSpeed")+" MHz"; 

            //Socket  Designation 
            SocketDesBox.Text = GetComponent("Win32_Processor", "SocketDesignation");

            //Cores and Threads
            CoresBox.Text = GetComponent("Win32_Processor", "NumberOfCores");
            ThreadsBox.Text = GetComponent("Win32_Processor", "ThreadCount");
            CountOfLogic.Text = GetComponent("Win32_Processor", "NumberOfLogicalProcessors");
            CountOfPhysic.Text = GetComponent("Win32_ComputerSystem", "NumberOfProcessors");

            //Physical Memory
            double totalforPhysical = Math.Round(Convert.ToDouble(GetComponent("Win32_OperatingSystem", "TotalVisibleMemorySize")) / (1024 * 1024), 2);
            TotalVisibleMemoryBox.Text = totalforPhysical.ToString() + " GBytes";

            //Virtual Memory
            double totalforVirtual = Math.Round(Convert.ToDouble(GetComponent("Win32_OperatingSystem", "TotalVirtualMemorySize")) / (1024 * 1024), 2);
            VirtualTotalMemoryBox.Text = totalforVirtual.ToString() + " GBytes";

            double availableforVirtual = Math.Round(Convert.ToDouble(GetComponent("Win32_OperatingSystem", "FreeVirtualMemory")) / (1024 * 1024), 2);
            VirtualAvailableMemoryBox.Text = availableforVirtual.ToString() + " GBytes";

            //Type Ram
            CreationClassName.Text = GetComponent("Win32_PhysicalMemory", "CreationClassName");
            PartNumber.Text = GetComponent("Win32_PhysicalMemory", "PartNumber");
            SerialNumber.Text = GetComponent("Win32_PhysicalMemory", "SerialNumber");
            TotalWidth.Text = GetComponent("Win32_PhysicalMemory", "TotalWidth");
            ClockSpeed.Text = GetComponent("Win32_PhysicalMemory", "ConfiguredClockSpeed") + " MHz";
            Voltage.Text = GetComponent("Win32_PhysicalMemory", "ConfiguredVoltage");
            ModuleManufacter.Text = GetComponent("Win32_PhysicalMemory", "Manufacturer");
            double modulesize = Math.Round(Convert.ToDouble(GetComponent("Win32_PhysicalMemory", "Capacity")) / (1024 * 1024 * 1024), 2);
            ModuleSize.Text = modulesize.ToString() + " GB";

            //MOtherboard
            MotherManufacterer.Text = GetComponent("Win32_BaseBoard", "Manufacturer");
            MotherModel.Text = GetComponent("Win32_BaseBoard", "Product");
            ModelVersion.Text = GetComponent("Win32_BaseBoard", "Version");
            MotherSerialNumber.Text = GetComponent("Win32_BaseBoard", "SerialNumber");
            BusSpecs.Text = GetComponent("Win32_MotherboardDevice", "PrimaryBusType") + " " + GetComponent("Win32_SystemSlot", "SlotDesignation");

            //BIOS
            BiosManufacturer.Text = GetComponent("Win32_BIOS ", "Manufacturer"); 
            BiosVersion.Text = GetComponent("Win32_BIOS ", "Name");
            BiosSerialNumber.Text = GetComponent("Win32_BIOS ", "SerialNumber");
            BiosStatus.Text = GetComponent("Win32_BIOS ", "Status");
            DateRelease.Text = GetComponent("Win32_BIOS ", "ReleaseDate");

            //GPU
            GPUName.Text = GetComponent("Win32_VideoController", "Name");
            GPUAdapterCompa.Text = GetComponent("Win32_VideoController", "AdapterCompatibility");
            GPUSystemNameLl.Text = GetComponent("Win32_VideoController", "SystemName");
            GPUStatus.Text = GetComponent("Win32_VideoController", "Status");

        }

         public string GetComponent(string key, string syntax) {

              ManagementObjectSearcher management_searcher = new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM " + key);

              try
              {
                foreach (var managementObj in management_searcher.Get()) {
                    try
                    {
                        return managementObj[syntax].ToString();
                    }
                    catch (Exception ex){
                        MessageBox.Show("Не удалось получить информацию", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
              }
              catch(Exception ex){
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }

              return null;

         }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            Date.Text = "Date: "+DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToShortTimeString();
            CPUBox.Text = (int)CPU.NextValue() + " " + "%";
            RAMBox.Text = (int)RAM.NextValue() + " " + "MBytes";
            PhysAvailableMemoryBox.Text = (int)RAM.NextValue() + " " + "MBytes";
            SystemUpTimeBox.Text = (int)SYS.NextValue() / 60 + " Minutes";
        }
    }
}
