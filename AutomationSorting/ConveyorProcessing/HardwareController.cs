using AutomationSorting.ConveyorProcessing.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace AutomationSorting.ConveyorProcessing
{
    public class HardwareController
    {
        public event EventHandler<NewUnitEventArgs> NewUnitEvent;
        public event EventHandler<NewBarcodeEventArgs> NewBarcodeEvent;//при получении баркода со сканера
        public event EventHandler<SortingCompletedEventArgs> SortingCompletedEvent;//при окончании обработки товара

        private static HardwareController _hardwareController = null;

        
        public Queue<byte[]> queue = new Queue<byte[]>();
        public volatile static List<string> ErrorInf;
        public volatile static List<string> LogInf;
        public bool Flag_ProductExist = false;
        public bool thread_stop = false;
        public int Mode = 0;
        public SerialPort serialPort_Controller = null;
        public SerialPort serialPort_Scaner = null;

        public HardwareController()
        {
            serialPort_Controller = new SerialPort();
            serialPort_Controller.PortName = "COM4";
            serialPort_Controller.BaudRate = 9600;
            serialPort_Controller.DataBits = 8;
            serialPort_Controller.StopBits = StopBits.One;
            serialPort_Controller.Parity = Parity.None;
            serialPort_Controller.DataReceived += new SerialDataReceivedEventHandler(serialPortController_DataReceived);

            serialPort_Scaner = new SerialPort();
            serialPort_Scaner.PortName = "COM3";
            serialPort_Scaner.BaudRate = 9600;
            serialPort_Scaner.DataBits = 8;
            serialPort_Scaner.StopBits = StopBits.One;
            serialPort_Scaner.Parity = Parity.None;
            serialPort_Scaner.DataReceived += new SerialDataReceivedEventHandler(serialPortScaner_DataReceived);
        }
        private string ByteArrayToHexString(byte data)
        {
            string sb = " ";
            sb = Convert.ToString(data, 16);
            return sb.ToString().ToUpper();
        }
        string tmp_str = "";
        bool result = false;
        private string  Str_PrintingTask = "" , 
                        Str_SortingError = "",
                        Str_SortingIndex = "",
                        Str_ScheduleSorting = "",
                        Str_ScheduleUnsorted = "";
        private int err;
        private int index = 0, point = 0;
        private long Time = 0;
        public void serialPortController_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] buffer = new byte[serialPort_Controller.ReadBufferSize];
                serialPort_Controller.Read(buffer, 0, serialPort_Controller.ReadBufferSize);

                /* try
                 {
                     int bytes = serialPort_Controller.BytesToRead + 0;
                     byte[] comBuffer = new byte[bytes];
                     serialPort_Controller.Read(comBuffer, 0, bytes);

                     for (int i_tmp = 0; i_tmp < bytes; i_tmp++)
                     {
                         string a = ByteArrayToHexString(comBuffer[i_tmp]);
                         string a_to_tmp_str = " ";
                         if ((result = a.Equals("00")) == true)
                             a_to_tmp_str = " ";
                         if ((result = a.Equals("20")) == true)
                             a_to_tmp_str = " ";
                         if ((result = a.Equals("21")) == true)
                             a_to_tmp_str = "!";
                         if ((result = a.Equals("23")) == true)
                             a_to_tmp_str = "#";
                         if ((result = a.Equals("24")) == true)
                             a_to_tmp_str = "$";
                         if ((result = a.Equals("25")) == true)
                             a_to_tmp_str = "%";
                         if ((result = a.Equals("26")) == true)
                             a_to_tmp_str = "&";
                         if ((result = a.Equals("27")) == true)
                             a_to_tmp_str = "'";
                         if ((result = a.Equals("28")) == true)
                             a_to_tmp_str = "(";
                         if ((result = a.Equals("29")) == true)
                             a_to_tmp_str = ")";
                         if ((result = a.Equals("2A")) == true)
                             a_to_tmp_str = "*";
                         if ((result = a.Equals("2B")) == true)
                             a_to_tmp_str = "+";
                         if ((result = a.Equals("2C")) == true)
                             a_to_tmp_str = ",";
                         if ((result = a.Equals("2D")) == true)
                             a_to_tmp_str = "-";
                         if ((result = a.Equals("2E")) == true)
                             a_to_tmp_str = ".";
                         if ((result = a.Equals("30")) == true)
                             a_to_tmp_str = "0";
                         if ((result = a.Equals("31")) == true)
                             a_to_tmp_str = "1";
                         if ((result = a.Equals("32")) == true)
                             a_to_tmp_str = "2";
                         if ((result = a.Equals("33")) == true)
                             a_to_tmp_str = "3";
                         if ((result = a.Equals("34")) == true)
                             a_to_tmp_str = "4";
                         if ((result = a.Equals("35")) == true)
                             a_to_tmp_str = "5";
                         if ((result = a.Equals("36")) == true)
                             a_to_tmp_str = "6";
                         if ((result = a.Equals("37")) == true)
                             a_to_tmp_str = "7";
                         if ((result = a.Equals("38")) == true)
                             a_to_tmp_str = "8";
                         if ((result = a.Equals("39")) == true)
                             a_to_tmp_str = "9";
                         if ((result = a.Equals("3A")) == true)
                             a_to_tmp_str = ":";
                         if ((result = a.Equals("3B")) == true)
                             a_to_tmp_str = ";";
                         if ((result = a.Equals("3C")) == true)
                             a_to_tmp_str = "<";
                         if ((result = a.Equals("3D")) == true)
                             a_to_tmp_str = "=";
                         if ((result = a.Equals("3E")) == true)
                             a_to_tmp_str = ">";
                         if ((result = a.Equals("3F")) == true)
                             a_to_tmp_str = "?";
                         if ((result = a.Equals("40")) == true)
                             a_to_tmp_str = "@";
                         if ((result = a.Equals("41")) == true)
                             a_to_tmp_str = "A";
                         if ((result = a.Equals("42")) == true)
                             a_to_tmp_str = "B";
                         if ((result = a.Equals("43")) == true)
                             a_to_tmp_str = "C";
                         if ((result = a.Equals("44")) == true)
                             a_to_tmp_str = "D";
                         if ((result = a.Equals("45")) == true)
                             a_to_tmp_str = "E";
                         if ((result = a.Equals("46")) == true)
                             a_to_tmp_str = "F";
                         if ((result = a.Equals("47")) == true)
                             a_to_tmp_str = "G";
                         if ((result = a.Equals("48")) == true)
                             a_to_tmp_str = "H";
                         if ((result = a.Equals("49")) == true)
                             a_to_tmp_str = "I";
                         if ((result = a.Equals("4A")) == true)
                             a_to_tmp_str = "J";
                         if ((result = a.Equals("4B")) == true)
                             a_to_tmp_str = "K";
                         if ((result = a.Equals("4C")) == true)
                             a_to_tmp_str = "L";
                         if ((result = a.Equals("4D")) == true)
                             a_to_tmp_str = "M";
                         if ((result = a.Equals("4E")) == true)
                             a_to_tmp_str = "N";
                         if ((result = a.Equals("4F")) == true)
                             a_to_tmp_str = "O";
                         if ((result = a.Equals("50")) == true)
                             a_to_tmp_str = "P";
                         if ((result = a.Equals("51")) == true)
                             a_to_tmp_str = "Q";
                         if ((result = a.Equals("52")) == true)
                             a_to_tmp_str = "R";
                         if ((result = a.Equals("53")) == true)
                             a_to_tmp_str = "S";
                         if ((result = a.Equals("54")) == true)
                             a_to_tmp_str = "T";
                         if ((result = a.Equals("55")) == true)
                             a_to_tmp_str = "U";
                         if ((result = a.Equals("56")) == true)
                             a_to_tmp_str = "V";
                         if ((result = a.Equals("57")) == true)
                             a_to_tmp_str = "W";
                         if ((result = a.Equals("58")) == true)
                             a_to_tmp_str = "X";
                         if ((result = a.Equals("59")) == true)
                             a_to_tmp_str = "Y";
                         if ((result = a.Equals("5A")) == true)
                             a_to_tmp_str = "Z";
                         if ((result = a.Equals("5B")) == true)
                             a_to_tmp_str = "[";
                         if ((result = a.Equals("5D")) == true)
                             a_to_tmp_str = "]";
                         if ((result = a.Equals("5E")) == true)
                             a_to_tmp_str = "^";
                         if ((result = a.Equals("5F")) == true)
                             a_to_tmp_str = "_";
                         if ((result = a.Equals("60")) == true)
                             a_to_tmp_str = "'";
                         if ((result = a.Equals("61")) == true)
                             a_to_tmp_str = "a";
                         if ((result = a.Equals("62")) == true)
                             a_to_tmp_str = "b";
                         if ((result = a.Equals("63")) == true)
                             a_to_tmp_str = "c";
                         if ((result = a.Equals("64")) == true)
                             a_to_tmp_str = "d";
                         if ((result = a.Equals("65")) == true)
                             a_to_tmp_str = "e";
                         if ((result = a.Equals("66")) == true)
                             a_to_tmp_str = "f";
                         if ((result = a.Equals("67")) == true)
                             a_to_tmp_str = "g";
                         if ((result = a.Equals("68")) == true)
                             a_to_tmp_str = "h";
                         if ((result = a.Equals("69")) == true)
                             a_to_tmp_str = "i";
                         if ((result = a.Equals("6A")) == true)
                             a_to_tmp_str = "j";
                         if ((result = a.Equals("6B")) == true)
                             a_to_tmp_str = "k";
                         if ((result = a.Equals("6C")) == true)
                             a_to_tmp_str = "l";
                         if ((result = a.Equals("6D")) == true)
                             a_to_tmp_str = "m";
                         if ((result = a.Equals("6E")) == true)
                             a_to_tmp_str = "n";
                         if ((result = a.Equals("6F")) == true)
                             a_to_tmp_str = "o";
                         if ((result = a.Equals("70")) == true)
                             a_to_tmp_str = "p";
                         if ((result = a.Equals("71")) == true)
                             a_to_tmp_str = "q";
                         if ((result = a.Equals("72")) == true)
                             a_to_tmp_str = "r";
                         if ((result = a.Equals("73")) == true)
                             a_to_tmp_str = "s";
                         if ((result = a.Equals("74")) == true)
                             a_to_tmp_str = "t";
                         if ((result = a.Equals("75")) == true)
                             a_to_tmp_str = "u";
                         if ((result = a.Equals("76")) == true)
                             a_to_tmp_str = "v";
                         if ((result = a.Equals("77")) == true)
                             a_to_tmp_str = "w";
                         if ((result = a.Equals("78")) == true)
                             a_to_tmp_str = "x";
                         if ((result = a.Equals("79")) == true)
                             a_to_tmp_str = "y";
                         if ((result = a.Equals("7A")) == true)
                             a_to_tmp_str = "z";
                         if ((result = a.Equals("7B")) == true)
                             a_to_tmp_str = "{";
                         if ((result = a.Equals("7C")) == true)
                             a_to_tmp_str = "|";
                         if ((result = a.Equals("7D")) == true)
                             a_to_tmp_str = "}";
                         if ((result = a.Equals("7E")) == true)
                             a_to_tmp_str = "~";
                         tmp_str += a_to_tmp_str;
                     }*/
                ProcessingResponse(buffer);
            }
            catch (Exception e1)
            {
                ErrorInf.Add("Ошибка при приеме информации по COM-порту! " + e1.Message.ToString());
            }
        }
        public void serialPortScaner_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] ScanBuffer = new byte[serialPort_Controller.ReadBufferSize];
                serialPort_Scaner.Read(ScanBuffer, 0, serialPort_Controller.ReadBufferSize);

                for (int i_tmp = 0; i_tmp < serialPort_Controller.ReadBufferSize; i_tmp++)
                {
                    string a = ByteArrayToHexString(ScanBuffer[i_tmp]);
                    string a_to_tmp_str = " ";
                    if ((result = a.Equals("00")) == true)
                        a_to_tmp_str = " ";
                    if ((result = a.Equals("20")) == true)
                        a_to_tmp_str = " ";
                    if ((result = a.Equals("21")) == true)
                        a_to_tmp_str = "!";
                    if ((result = a.Equals("23")) == true)
                        a_to_tmp_str = "#";
                    if ((result = a.Equals("24")) == true)
                        a_to_tmp_str = "$";
                    if ((result = a.Equals("25")) == true)
                        a_to_tmp_str = "%";
                    if ((result = a.Equals("26")) == true)
                        a_to_tmp_str = "&";
                    if ((result = a.Equals("27")) == true)
                        a_to_tmp_str = "'";
                    if ((result = a.Equals("28")) == true)
                        a_to_tmp_str = "(";
                    if ((result = a.Equals("29")) == true)
                        a_to_tmp_str = ")";
                    if ((result = a.Equals("2A")) == true)
                        a_to_tmp_str = "*";
                    if ((result = a.Equals("2B")) == true)
                        a_to_tmp_str = "+";
                    if ((result = a.Equals("2C")) == true)
                        a_to_tmp_str = ",";
                    if ((result = a.Equals("2D")) == true)
                        a_to_tmp_str = "-";
                    if ((result = a.Equals("2E")) == true)
                        a_to_tmp_str = ".";
                    if ((result = a.Equals("30")) == true)
                        a_to_tmp_str = "0";
                    if ((result = a.Equals("31")) == true)
                        a_to_tmp_str = "1";
                    if ((result = a.Equals("32")) == true)
                        a_to_tmp_str = "2";
                    if ((result = a.Equals("33")) == true)
                        a_to_tmp_str = "3";
                    if ((result = a.Equals("34")) == true)
                        a_to_tmp_str = "4";
                    if ((result = a.Equals("35")) == true)
                        a_to_tmp_str = "5";
                    if ((result = a.Equals("36")) == true)
                        a_to_tmp_str = "6";
                    if ((result = a.Equals("37")) == true)
                        a_to_tmp_str = "7";
                    if ((result = a.Equals("38")) == true)
                        a_to_tmp_str = "8";
                    if ((result = a.Equals("39")) == true)
                        a_to_tmp_str = "9";
                    if ((result = a.Equals("3A")) == true)
                        a_to_tmp_str = ":";
                    if ((result = a.Equals("3B")) == true)
                        a_to_tmp_str = ";";
                    if ((result = a.Equals("3C")) == true)
                        a_to_tmp_str = "<";
                    if ((result = a.Equals("3D")) == true)
                        a_to_tmp_str = "=";
                    if ((result = a.Equals("3E")) == true)
                        a_to_tmp_str = ">";
                    if ((result = a.Equals("3F")) == true)
                        a_to_tmp_str = "?";
                    if ((result = a.Equals("40")) == true)
                        a_to_tmp_str = "@";
                    if ((result = a.Equals("41")) == true)
                        a_to_tmp_str = "A";
                    if ((result = a.Equals("42")) == true)
                        a_to_tmp_str = "B";
                    if ((result = a.Equals("43")) == true)
                        a_to_tmp_str = "C";
                    if ((result = a.Equals("44")) == true)
                        a_to_tmp_str = "D";
                    if ((result = a.Equals("45")) == true)
                        a_to_tmp_str = "E";
                    if ((result = a.Equals("46")) == true)
                        a_to_tmp_str = "F";
                    if ((result = a.Equals("47")) == true)
                        a_to_tmp_str = "G";
                    if ((result = a.Equals("48")) == true)
                        a_to_tmp_str = "H";
                    if ((result = a.Equals("49")) == true)
                        a_to_tmp_str = "I";
                    if ((result = a.Equals("4A")) == true)
                        a_to_tmp_str = "J";
                    if ((result = a.Equals("4B")) == true)
                        a_to_tmp_str = "K";
                    if ((result = a.Equals("4C")) == true)
                        a_to_tmp_str = "L";
                    if ((result = a.Equals("4D")) == true)
                        a_to_tmp_str = "M";
                    if ((result = a.Equals("4E")) == true)
                        a_to_tmp_str = "N";
                    if ((result = a.Equals("4F")) == true)
                        a_to_tmp_str = "O";
                    if ((result = a.Equals("50")) == true)
                        a_to_tmp_str = "P";
                    if ((result = a.Equals("51")) == true)
                        a_to_tmp_str = "Q";
                    if ((result = a.Equals("52")) == true)
                        a_to_tmp_str = "R";
                    if ((result = a.Equals("53")) == true)
                        a_to_tmp_str = "S";
                    if ((result = a.Equals("54")) == true)
                        a_to_tmp_str = "T";
                    if ((result = a.Equals("55")) == true)
                        a_to_tmp_str = "U";
                    if ((result = a.Equals("56")) == true)
                        a_to_tmp_str = "V";
                    if ((result = a.Equals("57")) == true)
                        a_to_tmp_str = "W";
                    if ((result = a.Equals("58")) == true)
                        a_to_tmp_str = "X";
                    if ((result = a.Equals("59")) == true)
                        a_to_tmp_str = "Y";
                    if ((result = a.Equals("5A")) == true)
                        a_to_tmp_str = "Z";
                    if ((result = a.Equals("5B")) == true)
                        a_to_tmp_str = "[";
                    if ((result = a.Equals("5D")) == true)
                        a_to_tmp_str = "]";
                    if ((result = a.Equals("5E")) == true)
                        a_to_tmp_str = "^";
                    if ((result = a.Equals("5F")) == true)
                        a_to_tmp_str = "_";
                    if ((result = a.Equals("60")) == true)
                        a_to_tmp_str = "'";
                    if ((result = a.Equals("61")) == true)
                        a_to_tmp_str = "a";
                    if ((result = a.Equals("62")) == true)
                        a_to_tmp_str = "b";
                    if ((result = a.Equals("63")) == true)
                        a_to_tmp_str = "c";
                    if ((result = a.Equals("64")) == true)
                        a_to_tmp_str = "d";
                    if ((result = a.Equals("65")) == true)
                        a_to_tmp_str = "e";
                    if ((result = a.Equals("66")) == true)
                        a_to_tmp_str = "f";
                    if ((result = a.Equals("67")) == true)
                        a_to_tmp_str = "g";
                    if ((result = a.Equals("68")) == true)
                        a_to_tmp_str = "h";
                    if ((result = a.Equals("69")) == true)
                        a_to_tmp_str = "i";
                    if ((result = a.Equals("6A")) == true)
                        a_to_tmp_str = "j";
                    if ((result = a.Equals("6B")) == true)
                        a_to_tmp_str = "k";
                    if ((result = a.Equals("6C")) == true)
                        a_to_tmp_str = "l";
                    if ((result = a.Equals("6D")) == true)
                        a_to_tmp_str = "m";
                    if ((result = a.Equals("6E")) == true)
                        a_to_tmp_str = "n";
                    if ((result = a.Equals("6F")) == true)
                        a_to_tmp_str = "o";
                    if ((result = a.Equals("70")) == true)
                        a_to_tmp_str = "p";
                    if ((result = a.Equals("71")) == true)
                        a_to_tmp_str = "q";
                    if ((result = a.Equals("72")) == true)
                        a_to_tmp_str = "r";
                    if ((result = a.Equals("73")) == true)
                        a_to_tmp_str = "s";
                    if ((result = a.Equals("74")) == true)
                        a_to_tmp_str = "t";
                    if ((result = a.Equals("75")) == true)
                        a_to_tmp_str = "u";
                    if ((result = a.Equals("76")) == true)
                        a_to_tmp_str = "v";
                    if ((result = a.Equals("77")) == true)
                        a_to_tmp_str = "w";
                    if ((result = a.Equals("78")) == true)
                        a_to_tmp_str = "x";
                    if ((result = a.Equals("79")) == true)
                        a_to_tmp_str = "y";
                    if ((result = a.Equals("7A")) == true)
                        a_to_tmp_str = "z";
                    if ((result = a.Equals("7B")) == true)
                        a_to_tmp_str = "{";
                    if ((result = a.Equals("7C")) == true)
                        a_to_tmp_str = "|";
                    if ((result = a.Equals("7D")) == true)
                        a_to_tmp_str = "}";
                    if ((result = a.Equals("7E")) == true)
                        a_to_tmp_str = "~";
                    tmp_str += a_to_tmp_str;
                }

                OnNewBarcodeEvent(tmp_str);
            }
            catch (Exception e1)
            {
                ErrorInf.Add("Ошибка при приеме информации по COM-порту! " + e1.Message.ToString());
            }
        }
        public void ProcessingResponse(byte[] buf)//чтение откликов контроллера
        {
            if (buf[0] == 11)//ID устройства
            {
                if (buf[1] == 03)//функция чтения
                {
                    if (buf[2] == 02)//кол-во регистров для чтения *2
                    {
                        if (buf[3] == 00)
                        {
                            if (buf[4] == 00)//товара нет
                            {
                                Flag_ProductExist = false;
                            }
                            else if (buf[4] == 01)//есть товар
                            {
                                Flag_ProductExist = true;
                            }
                        }

                    }
                }
                else if (buf[1] == 06)//функция записи
                {
                    if ((buf[2] == 00) & (buf[3] == 00))//адрес регистра
                    {
                        if (buf[4] == 00)
                        {
                            if (buf[5] == 01)//перезаписан один регистр
                            {
                                thread_stop = true;//останавливаем поток передачи/приема
                                Mode = 0;//переходим в режим опроса контроллера на наличие товара
                            }
                        }
                    }

                }
            }
        }
                /*if (buf == "SortingCompletedEvent")
                    OnSortingCompletedEvent(Convert.ToInt16(str));
                if (buf.StartsWith(Str_PrintingTask) == true)//строка начинается с команды на печать 
                {
                    if (buf.IndexOf(",", Str_PrintingTask.Length) != -1)//первая запятая после команды на печать
                    {
                        point = buf.IndexOf(",", Str_PrintingTask.Length);
                        if (buf.IndexOf(",", Str_PrintingTask.Length + 2) != -1)//вторая запятая
                        {
                            index = Convert.ToInt16(buf.Substring(point++, buf.IndexOf(",", Str_PrintingTask.Length + 2) - point));//копируем подстроку длиной length
                            point = buf.IndexOf(",", Str_PrintingTask.Length + 2);//указатель на вторую запятую
                            Time = Convert.ToInt64(buf.Substring(point++));//копируем подстроку до конца вызываемой строки
                            SchedulePrintingTask(index, Time);//вызываем метод печати
                        }
                    }
                }

                if (buf.StartsWith(Str_SortingError) == true)//строка начинается с команды на ручную обработку 
                {
                    if (buf.IndexOf(",", Str_SortingError.Length) != -1)//первая запятая после команды на ручную обработку 
                    {
                        point = buf.IndexOf(",", Str_SortingError.Length);
                        if (buf.IndexOf(",", Str_SortingError.Length + 2) != -1)//вторая запятая
                        {
                            err = Convert.ToInt16(buf.Substring(point++, buf.IndexOf(",", Str_SortingError.Length + 2) - point));//копируем подстроку длиной length
                            point = buf.IndexOf(",", Str_SortingError.Length + 2);//указатель на вторую запятую
                            if (buf.IndexOf(",", point + 2) != -1)//третья запятая
                            {
                                index = Convert.ToInt16(buf.Substring(point++, buf.IndexOf(",", point + 2) - point));//копируем подстроку длиной length
                                point = buf.IndexOf(",", point + 2);//указатель на третью запятую
                                Time = Convert.ToInt64(buf.Substring(point++));//копируем подстроку до конца вызываемой строки
                                ScheduleSortingErrorProcessing((ProcessingErrorCodeEnum)err, index, Time);//вызываем метод ручной обработки
                            }
                        }
                    }
                }
                if (buf.StartsWith(Str_ScheduleSorting) == true)//строка начинается с команды на скидывание
                {
                    if (buf.IndexOf(",", Str_ScheduleSorting.Length) != -1)//первая запятая после команды на скидывание
                    {
                        point = buf.IndexOf(",", Str_ScheduleSorting.Length);
                        if (buf.IndexOf(",", Str_ScheduleSorting.Length + 2) != -1)//вторая запятая
                        {
                            Str_SortingIndex = buf.Substring(point++, buf.IndexOf(",", Str_ScheduleSorting.Length + 2) - point);//копируем подстроку длиной length
                            point = buf.IndexOf(",", Str_ScheduleSorting.Length + 2);//указатель на вторую запятую
                            if (buf.IndexOf(",", point + 2) != -1)//третья запятая
                            {
                                index = Convert.ToInt16(buf.Substring(point++, buf.IndexOf(",", point + 2) - point));//копируем подстроку длиной length
                                point = buf.IndexOf(",", point + 2);//указатель на третью запятую
                                Time = Convert.ToInt64(buf.Substring(point++));//копируем подстроку до конца вызываемой строки
                                ScheduleSorting(Str_SortingIndex, index, Time);//вызываем метод скидывания
                            }
                        }
                    }
                }
                if (buf.StartsWith(Str_ScheduleUnsorted) == true)//строка начинается с команды на несортированный товар 
                {
                    if (buf.IndexOf(",", Str_ScheduleUnsorted.Length) != -1)//первая запятая после команды на несортированный товар 
                    {
                        point = buf.IndexOf(",", Str_ScheduleUnsorted.Length);
                        if (buf.IndexOf(",", Str_ScheduleUnsorted.Length + 2) != -1)//вторая запятая
                        {
                            index = Convert.ToInt16(buf.Substring(point++, buf.IndexOf(",", Str_ScheduleUnsorted.Length + 2) - point));//копируем подстроку длиной length
                            point = buf.IndexOf(",", Str_ScheduleUnsorted.Length + 2);//указатель на вторую запятую
                            Time = Convert.ToInt64(buf.Substring(point++));//копируем подстроку до конца вызываемой строки
                            ScheduleUnsortedProcessing(index, Time);//вызываем метод несортированного товара
                        }
                    }
                }
            }
        }*/

        public static HardwareController GetInstance()
        {
            if (_hardwareController == null)
                _hardwareController = new HardwareController();
            
            return _hardwareController;
        }
        //включение события о получении нового бар-кода
        public void OnNewBarcodeEvent (string NewBarcode)
        {
            NewBarcodeEventArgs BarcodeEvent = new NewBarcodeEventArgs();
     
            if (NewBarcodeEvent != null)
            {
                BarcodeEvent.Barcode = NewBarcode;
                NewBarcodeEvent(this, BarcodeEvent);
            }
        }
        //включение события об окончании обработки товара
        public void OnSortingCompletedEvent(int NewIndex)
        {
            SortingCompletedEventArgs SortingCompleted = new SortingCompletedEventArgs();

            if (SortingCompletedEvent != null)
            {
                SortingCompleted.Index = NewIndex;
                SortingCompletedEvent(this, SortingCompleted);
            }
        }

        public void PreparePrinterLabel(string EPL, int index, long startProcessingTime)
        {

        }
        //передача принтеру этикетки
        public void SchedulePrintingTask(int index, long startProcessingTime)
        {
            //string strToSend = index.ToString() + "," +  startProcessingTime.ToString();
            //передача команды в очередь команд
            //queue.Enqueue(strToSend);
        }
        
        public void InitializeSortingIndexes(int index, string [] sortingIndexes)
        {

        }
        //увод продукта на линию ручной обработки
        public void ScheduleSortingErrorProcessing(ProcessingErrorCodeEnum error, int index, long startProcessingTime)
        {
            //string strToSend = error.ToString() + "," + index.ToString() + "," + startProcessingTime.ToString();
            //передача команды в очередь команд
            byte[] CommandSortingError = { 11, 06, 00, 00, 00, 06 };
            queue.Enqueue(CommandSortingError);
        }
        //скидывание
        public void ScheduleSorting(string sortingIndex, int index, long startProcessingTime)
        {
            //string strToSend = sortingIndex.ToString() + "," + index.ToString() + "," + startProcessingTime.ToString();
            //передача команды в очередь команд
            byte[] CommandSorting = { 11, 06, 00, 00, 00, 01 };
            queue.Enqueue(CommandSorting);
        }
        //отправка товара в паллет для несортированных товаров
        public void ScheduleUnsortedProcessing(int index, long startProcessingTime)
        {
            //string strToSend = index.ToString() + "," + startProcessingTime.ToString();
            //передача команды в очередь команд
            byte[] CommandUnsorted = { 11, 06, 00, 00, 00, 08 };
            queue.Enqueue(CommandUnsorted);
        }

    }
}
