﻿using Effortless.Net.Encryption;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.IO;
using Newtonsoft.Json;

namespace Szyfrator_9000
{
    public partial class Szyfrator : Form
    {


        byte[] key = new byte[] { 199, 232, 94, 247, 79, 106, 251, 170, 144, 144, 124, 57, 69, 53, 153, 145, 54, 176, 233, 98, 122, 122, 137, 47, 187, 245, 244, 140, 75, 16, 221, 228 };
        byte[] iv = new byte[] { 192, 23, 195, 102, 74, 146, 70, 26, 38, 164, 3, 189, 168, 102, 75, 86, 99, 44, 24, 85, 26, 192, 125, 112, 28, 200, 20, 173, 8, 255, 115, 25, };

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes("WRqGz'$}+%S=qgSmwM72?&v;a_6RganV");

        string content, path;

        List<password> passwords = new List<password>();

        

        private int offset;
        int index = 0;

        public Szyfrator()
        {
            InitializeComponent();
            SetupDataGridView();
        }

        private void Szyfrator_Load(object sender, EventArgs e)
        {

        }

        private void button_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); ;
                openFileDialog.Filter = "Pliki Szyfratora (*.szyfrator)|*.szyfrator|*.txt)|*.txt|Wszystkie Pliki (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    path = openFileDialog.FileName;

                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        content = reader.ReadToEnd();
                    }

                    string input = deszyfruj(content);
                    try { 
                    string pass = input.Substring(0, input.IndexOf("|password|"));
                    string json = input.Substring(input.IndexOf("|password|") + 10);
                    if (pass.Length > 0)
                    {
                        login checkPWD = new login();
                        checkPWD.Show();
                        checkPWD.FormClosed += delegate
                          {
                              if (password.checkPassword(login.getPassword(), pass))
                              {
                                  password.setPassword(pass);
                                  passwords = JsonConvert.DeserializeObject<List<password>>(json);
                                  PopulateDataGridView(passwords);
                              }
                              else
                              {
                                  MessageBox.Show("Wrong Password try Again", "Wrong Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                              }

                          };
                    } }
                    catch (Exception)
                    {
                        MessageBox.Show("This Isn't A Encryptor File !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
            }


        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Szyfrator_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void tekst_TextChanged(object sender, EventArgs e)
        {

        }

        private void zapisz_Click(object sender, EventArgs e)
        {
            if (password.getPassword() != "")
            {

                //byte[] byteArray = Encoding.UTF8.GetBytes(szyfruj(json));
                string json = password.getPassword() + "|password|" + getdata();
                byte[] byteArray = Encoding.UTF8.GetBytes(szyfruj(json));

                Stream myStream;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter = "Pliki Szyfratora (*.szyfrator)|*.szyfrator|*.txt)|*.txt|Wszystkie Pliki (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        myStream.Write(byteArray, 0, byteArray.Length);
                        myStream.Close();
                    }
                }

            }
            else
            {
                MessageBox.Show("You can't Save Empty file!", "Empty file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        string deszyfruj(string zaszyfrowane)
        {
            try
            {
                return Strings.Decrypt(zaszyfrowane, key, iv);
            }
            catch (Exception)
            {
                MessageBox.Show("Error 404", "Nadpisano", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "Error 404";
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string json = "[{'id':0,'name':'nazwa','login':'Test','password':'Ukryte byczq','email':'Test@gmail.com','notes':'Jakieś notatki'},{'id':1,'name':'nazwa','login':'Test','password':'Ukryte Byczq','email':'Test@gmial.com','notes':'Jakieś notatki'},{'id':2,'name':'nazwa','login':'Test','password':'Ukryte Byczq','email':'Test@gmial.com','notes':'Jakieś notatki'},{'id':3,'name':'nazwa','login':'Test','password':'Ukryte Byczq','email':'Test@gmial.com','notes':'Jakieś notatki'},{'id':4,'name':'nazwa','login':'Test','password':'Ukryte Byczq','email':'Test@gmial.com','notes':'Jakieś notatki'},{'id':5,'name':'nazwa','login':'Test','password':'Ukryte Byczq','email':'Test@gmial.com','notes':'Jakieś notatki'},{'id':6,'name':'nazwa','login':'Test','password':'Ukryte Byczq','email':'Test@gmial.com','notes':'Jakieś notatki'}]";
            passwords = JsonConvert.DeserializeObject<List<password>>(json);
            //Console.WriteLine(passwords[2].Login );


            PopulateDataGridView(passwords);

            string json2 = JsonConvert.SerializeObject(passwords);

            //Console.WriteLine(json2);



        }

        string szyfruj(string odszyfrowane)
        {
            try
            {
                return Strings.Encrypt(odszyfrowane, key, iv); ;
            }
            catch (Exception)
            {
                MessageBox.Show("Error 405", "Error Zapisu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "Error 405";
            }


        }

        private void passwords_CellFormatting(object sender,
        System.Windows.Forms.DataGridViewCellFormattingEventArgs e)
        {
            if (e != null)
            {
                if (this.passwordsDataGridView.Columns[e.ColumnIndex].Name == "Release Date")
                {
                    if (e.Value != null)
                    {
                        try
                        {
                            e.Value = DateTime.Parse(e.Value.ToString())
                                .ToLongDateString();
                            e.FormattingApplied = true;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("{0} is not a valid date.", e.Value.ToString());
                        }
                    }
                }
            }
        }

       

    

        private void SetupDataGridView()
        {
            //this.Controls.Add(passwordsDataGridView);

            passwordsDataGridView.ColumnCount = 7;

            passwordsDataGridView.Name = "passwordsDataGridView";
            //passwordsDataGridView.Location = new Point(8, 8);
            //passwordsDataGridView.Size = new Size(500, 250);
            passwordsDataGridView.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            passwordsDataGridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            passwordsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            passwordsDataGridView.GridColor = Color.Black;
            passwordsDataGridView.RowHeadersVisible = false;


            passwordsDataGridView.CellFormatting += new
                DataGridViewCellFormattingEventHandler(
                passwords_CellFormatting);
        }

        private void passwordsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Unhidepwd(sender, e );
        }

        private void Unhidepwd(object sender, DataGridViewCellEventArgs e)
        {
            if ((e != null) && (e.RowIndex >= 0))
            {
                if ((e != null) && (e.RowIndex >= 0))
                {
                    if (passwordsDataGridView.Rows[e.RowIndex].Cells[5].Value != null)
                    {
                        passwordsDataGridView.Rows[e.RowIndex].Cells[4].Value = passwordsDataGridView.Rows[e.RowIndex].Cells[5].Value;
                        Console.WriteLine(passwordsDataGridView.Rows[e.RowIndex].Cells[4].Value);
                    }

                }
            }
        }

        private void passwordsDataGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if ((e != null) && (e.RowIndex >= 0))
            {
                if (passwordsDataGridView.Rows[e.RowIndex].Cells[4].Value != null)
                {
                    if (!passwordsDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString().Contains('\u25cf'))
                    {
                        passwordsDataGridView.Rows[e.RowIndex].Cells[5].Value = passwordsDataGridView.Rows[e.RowIndex].Cells[4].Value;
                        passwordsDataGridView.Rows[e.RowIndex].Cells[4].Value = new String('\u25cf', passwordsDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString().Length);
                        Console.WriteLine(passwordsDataGridView.Rows[e.RowIndex].Cells[4].Value);
                    }

                }
            }
        }

        private void passwordsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Unhidepwd(sender , e);
        }

        private void PopulateDataGridView(List<password> passwords)
        {
            for (int i = 0; i < passwords.Count; i++)
            {
                index += 1;
                passwordsDataGridView.Rows.Add(index.ToString(), passwords[i].Name, passwords[i].Login, passwords[i].Email, new String('\u25CF', passwords[i].Password.Length), passwords[i].Password, passwords[i].Notes);
            }
            

        }

        private void Dodaj_Click(object sender, EventArgs e)
        {
            if (password.getPassword() == "")
            {
                newPassword newPWD = new newPassword();
                newPWD.Show();
                newPWD.FormClosed += delegate
                {
                    password.sethash(newPassword.getPassword());
                    if (password.getPassword() != "")
                    {
                        additem();
                    }
                };
            }
            else
            {
                additem();
            }
        }

        private void additem()
        {
            AddWindow add = new AddWindow();
            add.Show();
            add.FormClosed += delegate
            {
                if (add.NewItem != null)
                {
                    index += 1;
                    passwordsDataGridView.Rows.Add(index.ToString(), add.NewItem.Name, add.NewItem.Login, add.NewItem.Email, new String('\u25CF', add.NewItem.Password.Length), add.NewItem.Password, add.NewItem.Notes);
                }
                //MessageBox.Show("subForm has closed");
            };
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            
            
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            if (this.passwordsDataGridView.SelectedRows.Count > 0)
            {
                DialogResult zapytanie = MessageBox.Show("Are you Wan't To Delete ?", "Deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (zapytanie == DialogResult.Yes)
                {
                    this.passwordsDataGridView.Rows.RemoveAt(
                        this.passwordsDataGridView.SelectedRows[0].Index);
                }
            }
            else
            {
                MessageBox.Show("Set Passwords what you wan't to delete","Error Deleting",MessageBoxButtons.OK , MessageBoxIcon.Error);
            }
        }

        string getdata()
        {
            List<password> data = new List<password>(offset);

            for (int i = 0; i < passwordsDataGridView.Rows.Count; i++)
            {
                if (passwordsDataGridView.Rows[i] != null) 
                {
                    password item = new password();

                    if (passwordsDataGridView.Rows[i].Cells[0].Value != null)
                    {
                        item.id = Convert.ToInt32(passwordsDataGridView.Rows[i].Cells[0].Value);
                    }
                    else
                    {
                        item.id = 0;
                    }

                    if (passwordsDataGridView.Rows[i].Cells[1].Value != null)
                    {
                        item.Name = passwordsDataGridView.Rows[i].Cells[1].Value.ToString();
                    }
                    else
                    {
                        item.Name = "";
                    }

                    if (passwordsDataGridView.Rows[i].Cells[2].Value != null)
                    {
                        item.Login = passwordsDataGridView.Rows[i].Cells[2].Value.ToString();
                    }
                    else
                    {
                        item.Login = "";
                    }
                    if (passwordsDataGridView.Rows[i].Cells[3].Value != null)

                    {

                        item.Email = passwordsDataGridView.Rows[i].Cells[3].Value.ToString();
                    }
                    else
                    {
                        item.Email = "";
                    }
                    if (passwordsDataGridView.Rows[i].Cells[4].Value != null)
                    {
                        item.Password = passwordsDataGridView.Rows[i].Cells[5].Value.ToString();
                    }
                    else
                    {
                        item.Password = "";
                    }
                    if (passwordsDataGridView.Rows[i].Cells[5].Value != null)
                    {
                        item.Notes = passwordsDataGridView.Rows[i].Cells[6].Value.ToString();
                    }
                    else
                    {
                        item.Notes = "";
                    }
                   
                    data.Add(item);
                }
                
                }

            return JsonConvert.SerializeObject(data);
        }

    }
}

