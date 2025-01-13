using System;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace IPaddress
{
	public partial class MainForm : Form
	{
		private Label label1;
		private Label label2;
		private Label label3;
		private Label label4;
		private TextBox txtIpAddress;
		private ComboBox cmbMask; // Заменяею TextBox на ComboBox что бы убрать баг с вводом подсети
		private TextBox txtPrefix;
		private TextBox txtInfo;
		private Button btnCalculate;
		private Button btnExpand; // Кнопка для разложения

		public MainForm()
		{
			InitializeComponent();
			txtIpAddress.Text = "192.168.1.1"; // Устанавливаем IP по умолчанию
			PopulateMaskOptions(); // Заполняем ComboBox на старте
		}

		private void PopulateMaskOptions()
		{
			string[] masks = {
				"255.255.255.0",
				"255.255.255.128",
				"255.255.255.192",
				"255.255.255.224",
				"255.255.255.240",
				"255.255.255.248",
				"255.255.255.252",
				"255.255.0.0",
				"255.255.255.255",
				"255.0.0.0"
			};

			cmbMask.Items.AddRange(masks);
			cmbMask.SelectedIndex = 0; // Устанавливаем первый элемент по умолчанию
		}

		private void InitializeComponent()
		{
			this.label1 = new Label();
			this.label2 = new Label();
			this.label3 = new Label();
			this.label4 = new Label();
			this.txtIpAddress = new TextBox();
			this.cmbMask = new ComboBox(); // Используем ComboBox вместо TextBox
			this.txtPrefix = new TextBox();
			this.txtInfo = new TextBox();
			this.btnCalculate = new Button();
			this.btnExpand = new Button();
			this.SuspendLayout();

			// label1
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(84, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Ввод IP-адреса";

			// label2
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 47);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(67, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Ввод маски";

			// label3
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 89);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(85, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Ввод префикса";

			// label4
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(13, 167);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(108, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "Информации о сети";

			// txtIpAddress
			this.txtIpAddress.Location = new System.Drawing.Point(12, 25);
			this.txtIpAddress.Name = "txtIpAddress";
			this.txtIpAddress.Size = new System.Drawing.Size(306, 20);
			this.txtIpAddress.TabIndex = 4;
			this.txtIpAddress.TextChanged += new EventHandler(this.txtIpAddress_TextChanged);

			// cmbMask
			this.cmbMask.Location = new System.Drawing.Point(12, 66);
			this.cmbMask.Name = "cmbMask";
			this.cmbMask.Size = new System.Drawing.Size(306, 21);
			this.cmbMask.TabIndex = 5;
			this.cmbMask.SelectedIndexChanged += new EventHandler(this.cmbMask_SelectedIndexChanged);

			// txtPrefix
			this.txtPrefix.Location = new System.Drawing.Point(12, 105);
			this.txtPrefix.Name = "txtPrefix";
			this.txtPrefix.Size = new System.Drawing.Size(306, 20);
			this.txtPrefix.TabIndex = 6;

			// txtInfo
			this.txtInfo.Location = new System.Drawing.Point(12, 183);
			this.txtInfo.Multiline = true;
			this.txtInfo.Name = "txtInfo";
			this.txtInfo.Size = new System.Drawing.Size(306, 66);
			this.txtInfo.TabIndex = 7;

			// btnCalculate
			this.btnCalculate.Location = new System.Drawing.Point(12, 141);
			this.btnCalculate.Name = "btnCalculate";
			this.btnCalculate.Size = new System.Drawing.Size(150, 23);
			this.btnCalculate.TabIndex = 8;
			this.btnCalculate.Text = "Расчитать";
			this.btnCalculate.UseVisualStyleBackColor = true;
			this.btnCalculate.Click += new EventHandler(this.btnCalculate_Click_1);

			// btnExpand
			this.btnExpand.Location = new System.Drawing.Point(168, 141);
			this.btnExpand.Name = "btnExpand";
			this.btnExpand.Size = new System.Drawing.Size(150, 23);
			this.btnExpand.TabIndex = 9;
			this.btnExpand.Text = "Разложить";
			this.btnExpand.UseVisualStyleBackColor = true;
			this.btnExpand.Click += new EventHandler(this.btnExpand_Click);

			// MainForm
			this.ClientSize = new System.Drawing.Size(339, 278);
			this.Controls.Add(this.btnExpand);
			this.Controls.Add(this.btnCalculate);
			this.Controls.Add(this.txtInfo);
			this.Controls.Add(this.txtPrefix);
			this.Controls.Add(this.cmbMask); // Добавляем ComboBox на форму
			this.Controls.Add(this.txtIpAddress);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "MainForm";
			this.Text = "IP Calc";
			this.Load += new EventHandler(this.MainForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		private void cmbMask_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Обновляем префикс на основе выбранной маски
			if (cmbMask.SelectedItem is string selectedMask && IPAddress.TryParse(selectedMask, out IPAddress mask))
			{
				txtPrefix.Text = GetPrefixLength(mask).ToString();
			}
		}

		private void btnCalculate_Click_1(object sender, EventArgs e)
		{
			try
			{
				string ip = txtIpAddress.Text;
				string mask = cmbMask.SelectedItem.ToString(); // Получаем выбранную маску с ComboBox

				if (IPAddress.TryParse(ip, out IPAddress ipAddress) && IPAddress.TryParse(mask, out IPAddress subnetMask))
				{
					var network = GetNetworkAddress(ipAddress, subnetMask);
					var broadcast = GetBroadcastAddress(ipAddress, subnetMask);
					var nodeCount = GetNodeCount(subnetMask);

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

		private void btnExpand_Click(object sender, EventArgs e)
		{
			// Создаем новое окно для отображения подсетей
			SubnetForm subnetForm = new SubnetForm(txtIpAddress.Text, cmbMask.SelectedItem.ToString());
			subnetForm.Show();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
		}

		private void txtIpAddress_TextChanged(object sender, EventArgs e)
		{
		}

		private IPAddress GetNetworkAddress(IPAddress ip, IPAddress mask)
		{
			byte[] ipBytes = ip.GetAddressBytes();
			byte[] maskBytes = mask.GetAddressBytes();
			byte[] networkBytes = new byte[ipBytes.Length];

			for (int i = 0; i < ipBytes.Length; i++)
			{
				networkBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
			}

			return new IPAddress(networkBytes);
		}

		private IPAddress GetBroadcastAddress(IPAddress ip, IPAddress mask)
		{
			byte[] ipBytes = ip.GetAddressBytes();
			byte[] maskBytes = mask.GetAddressBytes();
			byte[] broadcastBytes = new byte[ipBytes.Length];

			for (int i = 0; i < ipBytes.Length; i++)
			{
				broadcastBytes[i] = (byte)(ipBytes[i] | ~maskBytes[i]);
			}

			return new IPAddress(broadcastBytes);
		}

		private int GetNodeCount(IPAddress mask)
		{
			byte[] maskBytes = mask.GetAddressBytes();
			int numZeroBits = 0;

			foreach (var b in maskBytes)
			{
				numZeroBits += CountZeroBits(b);
			}

			return (1 << numZeroBits) - 2; // Возвращает количество узлов
		}

		private int CountZeroBits(byte b)
		{
			int count = 0;
			for (int i = 0; i < 8; i++)
			{
				if ((b & (1 << i)) == 0)
				{
					count++;
				}
			}
			return count;
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
			return prefixLength; // Если маска является полным 32-битным числом
		}
	}

	public class SubnetForm : Form
	{
		private Label lblSubnetInfo;
		private Button btnSave;
		private string ipAddress;
		private string subnetMask;
		private TextBox txtSubnetList;

		public SubnetForm(string ip, string mask)
		{
			this.ipAddress = ip;
			this.subnetMask = mask;
			InitializeComponent();
			LoadSubnets();
		}

		private void InitializeComponent()
		{
			this.lblSubnetInfo = new Label();
			this.btnSave = new Button();
			this.txtSubnetList = new TextBox();

			this.SuspendLayout();
			// 
			// lblSubnetInfo
			// 
			this.lblSubnetInfo.AutoSize = true;
			this.lblSubnetInfo.Location = new System.Drawing.Point(12, 9);
			this.lblSubnetInfo.Name = "lblSubnetInfo";
			this.lblSubnetInfo.Size = new System.Drawing.Size(0, 13);
			this.lblSubnetInfo.TabIndex = 0;
			// 
			// txtSubnetList
			// 
			this.txtSubnetList.Location = new System.Drawing.Point(12, 56); // Начальное положение
			this.txtSubnetList.Multiline = true;
			this.txtSubnetList.Name = "txtSubnetList";
			this.txtSubnetList.ReadOnly = true;
			this.txtSubnetList.ScrollBars = ScrollBars.Vertical; // Включение прокрутки
			this.txtSubnetList.Size = new System.Drawing.Size(360, 150);
			this.txtSubnetList.TabIndex = 1;
			// Установка свойства Anchor для txtSubnetList что бы при раскрытии на весь экран работало норм
			this.txtSubnetList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(300, 12); // Положение кнопки в правом верхнем углу
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23); // Размер кнопки
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "Сохранить";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new EventHandler(this.btnSave_Click);

			// 
			// SubnetForm
			// 
			this.ClientSize = new System.Drawing.Size(384, 221);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.txtSubnetList);
			this.Controls.Add(this.lblSubnetInfo);
			this.Name = "SubnetForm";
			this.Text = "Подсети";
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		private void LoadSubnets()
		{
			if (!IPAddress.TryParse(ipAddress, out IPAddress ip) ||
				!IPAddress.TryParse(subnetMask, out IPAddress mask))
			{
				txtSubnetList.Text = "Некорректный IP-адрес или маска сети.";
				return;
			}

			byte[] ipBytes = ip.GetAddressBytes();
			byte[] maskBytes = mask.GetAddressBytes();
			int prefixLength = GetPrefixLength(mask);
			int numberOfSubnets = 1 << (32 - prefixLength); // Количество подсетей на основе ширины сети.

			StringBuilder subnetInfo = new StringBuilder();
			subnetInfo.AppendLine($"Подсети для {ipAddress} с маской {subnetMask}:");

			// Генерация подсетей
			for (int i = 0; i < numberOfSubnets; i++)
			{
				byte[] subnetBytes = new byte[ipBytes.Length];
				for (int j = 0; j < ipBytes.Length; j++)
				{
					subnetBytes[j] = (byte)(ipBytes[j] & maskBytes[j]);
				}

				// Определение номера подсети
				subnetBytes[3] += (byte)i;
				IPAddress subnetAddress = new IPAddress(subnetBytes);

				// Формирование строки с адресом подсети
				subnetInfo.AppendLine($"Подсеть {i + 1}: {subnetAddress}");
			}

			// Отображение подсетей в текстовом поле
			txtSubnetList.Text = subnetInfo.ToString();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			// Сохранение информации в файл
			try
			{
				// Проверяем, что текст не слишком большой
				if (txtSubnetList.Text.Length > 0)
				{
					using (System.IO.StreamWriter writer = new System.IO.StreamWriter("subnets.txt"))
					{
						writer.WriteLine(txtSubnetList.Text);
					}
					MessageBox.Show("Информация о подсетях сохранена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					MessageBox.Show("Нет данных для сохранения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}
			catch (OutOfMemoryException)
			{
				MessageBox.Show("Ошибка: Превышен предел памяти.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Ошибка при сохранении: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
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
						return prefixLength; // Возвращаем длину префикса, когда находим ноль
					}
					prefixLength++;
				}
			}
			return prefixLength; // Если маска является полным 32-битным числом
		}
	}
}