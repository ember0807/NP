using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPaddress
{
	public partial class MainForm : Form
	{
		private Label label2;
		private Label label3;
		private Label label4;
		private TextBox txtIpAddress;
		private TextBox txtMask;
		private TextBox txtPrefix;
		private TextBox txtInfo;
		private Button btnCalculate;
		private Label label1;

		public MainForm()
		{
			InitializeComponent();

			// Задаем значения по умолчанию
			txtIpAddress.Text = "192.168.1.1";  // Пример IP-адреса
			txtMask.Text = "255.255.255.0";     // Пример маски сети
		}

		

		private void txtMask_TextChanged(object sender, EventArgs e)
		{
			if (IPAddress.TryParse(txtMask.Text, out IPAddress mask))
			{
				txtPrefix.Text = GetPrefixLength(mask).ToString();
			}
		}

		private void txtPrefix_TextChanged(object sender, EventArgs e)
		{
			if (int.TryParse(txtPrefix.Text, out int prefix))
			{
				txtMask.Text = GetMaskFromPrefix(prefix).ToString();
			}
		}
		// Метод для вычисления адреса сети на основе IP и маски
		private IPAddress GetNetworkAddress(IPAddress ip, IPAddress mask)
		{
			byte[] ipBytes = ip.GetAddressBytes(); // Получаем байты IP-адреса.
			byte[] maskBytes = mask.GetAddressBytes(); // Получаем байты маски сети.
			byte[] networkBytes = new byte[ipBytes.Length]; // Создаем массив для хранения байтов адреса сети.

			for (int i = 0; i < ipBytes.Length; i++)
			{
				networkBytes[i] = (byte)(ipBytes[i] & maskBytes[i]); // Применяем побитовую операцию AND для каждого байта.
			}

			return new IPAddress(networkBytes); // Возвращаем IP-адрес, представляющий адрес сети.
		}
		// Метод для вычисления широковещательного адреса
		private IPAddress GetBroadcastAddress(IPAddress ip, IPAddress mask)
		{
			byte[] ipBytes = ip.GetAddressBytes(); // Получаем байты IP-адреса.
			byte[] maskBytes = mask.GetAddressBytes(); // Получаем байты маски сети.
			byte[] broadcastBytes = new byte[ipBytes.Length]; // Создаем массив для хранения байтов широковещательного адреса.

			for (int i = 0; i < ipBytes.Length; i++)
			{
				//Если один из битов равен 1, то результат будет 1.
				//Если оба бита равны 0, то результат будет 0.
				broadcastBytes[i] = (byte)(ipBytes[i] | ~maskBytes[i]); // Применяем побитовую операцию OR с инверсией маски.
			}

			return new IPAddress(broadcastBytes); // Возвращаем IP-адрес, представляющий широковещательный адрес.
		}

		// Метод для вычисления количества узлов в сети на основе маски
		private int GetNodeCount(IPAddress mask)
		{
			byte[] maskBytes = mask.GetAddressBytes(); // Получаем байты маски сети.
			int numZeroBits = 0; // Переменная для подсчета нулевых битов.

			foreach (var b in maskBytes) // Для каждого байта маски
			{
				numZeroBits += CountZeroBits(b); // Считаем количество нулевых битов и добавляем к общему счетчику.
			}

			return (1 << numZeroBits) - 2; // Возвращаем количество узлов, исключая адрес сети и широковещательный адрес.
		}

		// Метод для подсчета нулевых битов в байте.
		private int CountZeroBits(byte b)
		{
			int count = 0; // Инициализация счетчика.
			for (int i = 0; i < 8; i++) // Проверяем каждый бит в байте.
			{
				if ((b & (1 << i)) == 0) // Если бит равен 0, увеличиваем счетчик.
				{
					count++;
				}
			}
			return count; // Возвращаем количество нулевых битов.
		}

		private int GetPrefixLength(IPAddress mask)
		{
			byte[] maskBytes = mask.GetAddressBytes();
			int prefixLength = 0;

			foreach (var b in maskBytes)
			{
				for (int i = 7; i >= 0; i--)
				{
					if ((b & (1 << i)) == 0)
					{
						return prefixLength;
					}
					prefixLength++;
				}
			}
			return prefixLength;
		}

		private IPAddress GetMaskFromPrefix(int prefix)
		{
			uint mask = uint.MaxValue << (32 - prefix);
			return new IPAddress(BitConverter.GetBytes(mask).Reverse().ToArray());
		}

		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtIpAddress = new System.Windows.Forms.TextBox();
			this.txtMask = new System.Windows.Forms.TextBox();
			this.txtPrefix = new System.Windows.Forms.TextBox();
			this.txtInfo = new System.Windows.Forms.TextBox();
			this.btnCalculate = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(84, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Ввод IP-адреса";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 47);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(67, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Ввод маски";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 89);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(85, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Ввод префикса";
			this.label3.Click += new System.EventHandler(this.label3_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(13, 167);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(108, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "Информации о сети";
			// 
			// txtIpAddress
			// 
			this.txtIpAddress.Location = new System.Drawing.Point(12, 25);
			this.txtIpAddress.Name = "txtIpAddress";
			this.txtIpAddress.Size = new System.Drawing.Size(306, 20);
			this.txtIpAddress.TabIndex = 4;
			this.txtIpAddress.TextChanged += new System.EventHandler(this.txtIpAddress_TextChanged);
			// 
			// txtMask
			// 
			this.txtMask.Location = new System.Drawing.Point(12, 66);
			this.txtMask.Name = "txtMask";
			this.txtMask.Size = new System.Drawing.Size(306, 20);
			this.txtMask.TabIndex = 5;
			// 
			// txtPrefix
			// 
			this.txtPrefix.Location = new System.Drawing.Point(12, 105);
			this.txtPrefix.Name = "txtPrefix";
			this.txtPrefix.Size = new System.Drawing.Size(306, 20);
			this.txtPrefix.TabIndex = 6;
			// 
			// txtInfo
			// 
			this.txtInfo.Location = new System.Drawing.Point(12, 183);
			this.txtInfo.Multiline = true;
			this.txtInfo.Name = "txtInfo";
			this.txtInfo.Size = new System.Drawing.Size(306, 66);
			this.txtInfo.TabIndex = 7;
			// 
			// btnCalculate
			// 
			this.btnCalculate.Location = new System.Drawing.Point(12, 141);
			this.btnCalculate.Name = "btnCalculate";
			this.btnCalculate.Size = new System.Drawing.Size(306, 23);
			this.btnCalculate.TabIndex = 8;
			this.btnCalculate.Text = "Расчитать";
			this.btnCalculate.UseVisualStyleBackColor = true;
			this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click_1);
			// 
			// MainForm
			// 
			this.ClientSize = new System.Drawing.Size(339, 278);
			this.Controls.Add(this.btnCalculate);
			this.Controls.Add(this.txtInfo);
			this.Controls.Add(this.txtPrefix);
			this.Controls.Add(this.txtMask);
			this.Controls.Add(this.txtIpAddress);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "MainForm";
			this.Text = "IP Calc";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private void label3_Click(object sender, EventArgs e)
		{

		}

		private void txtIpAddress_TextChanged(object sender, EventArgs e)
		{

		}

		private void MainForm_Load(object sender, EventArgs e)
		{

		}

		private void btnCalculate_Click_1(object sender, EventArgs e)
		{
			try
			{
				string ip = txtIpAddress.Text;
				string mask = txtMask.Text;

				if (IPAddress.TryParse(ip, out IPAddress ipAddress) && IPAddress.TryParse(mask, out IPAddress subnetMask))
				{
					var network = GetNetworkAddress(ipAddress, subnetMask);
					var broadcast = GetBroadcastAddress(ipAddress, subnetMask);
					var nodeCount = GetNodeCount(subnetMask);


					//MessageBox.Show($"Network Address: {network}\nBroadcast Address: {broadcast}\nNode Count: {nodeCount}");

					txtInfo.Text = $"Адрес сети: {network}{Environment.NewLine}" +
			   $"Широковещательный адрес: {broadcast}{Environment.NewLine}" +
			   $"Количество узлов сети: {nodeCount}";

					// Обновляем префикс
					txtPrefix.Text = GetPrefixLength(subnetMask).ToString();
				}
				else
				{
					MessageBox.Show("Некорректный IP-адрес или маска сети!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
