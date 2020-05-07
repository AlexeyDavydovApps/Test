using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CW_DB_1
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
    // Установка соединения с базой данных:
            private void button1_Click(object sender, EventArgs e)
            {
                myConnection = new SqlConnection("server=1-ПК;" + "Trusted_Connection=yes;" + "database=master;" + "connection timeout=30");
                try
                {
                    myConnection.Open();
                    label1.Text = "Соединение установлено"; button1.Enabled = false;
                              catch (Exception exp)
                {
                    label1.Text = exp.ToString();
                }
            }
            //Метод SqlConnection.Open - Открывает подключение к базе данных со значениями свойств, определяемыми объектом ConnectionString.


//Загрузка данных для заполнения журнала на экране данными:
private void FillTheTable1()
        {
            string tab = comboBox3.Text; try
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("select kol_z, kol_l from discipline where name_discip='" + tab + "'", myConnection);
                myReader = myCommand.ExecuteReader(); while (myReader.Read())
                {

                    col_z = Convert.ToInt32(myReader["kol_z"]); col_l = Convert.ToInt32(myReader["kol_l"]);
                }
                myReader.Close();
                for (int j = 0; j < col_z; j++)
                {
                    dataGridView1.Columns.Add("z" + (j + 1), (j + 1).ToString()); dataGridView1.Columns["z" + (j + 1)].Width = 50;
                }
                for (int j = 0; j < col_l; j++)
                {
                    dataGridView1.Columns.Add("l" + (j + 1), "ЛР" + (j + 1)); dataGridView1.Columns["l" + (j + 1)].Width = 50;
                }

                myReader = null;
                myCommand = new SqlCommand("select name_discip, cod_z, mark, name_prepod, name
                " +
                "from eregister " +
                "join discipline on name_ds=discipline.cod join teachers on
                name_pr = teachers.cod join students on name_st = students.nomer " +
                "where name_discip='" + tab + "'", myConnection); myReader = myCommand.ExecuteReader();
                int i = 0; int k = 0;
                bool fl = true;
                while (myReader.Read())
                {
                    int p = myReader.Depth; if (fl)
                    {
                        dataGridView1.Rows.Add(); dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString(); dataGridView1[0, i].Value = myReader["name"].ToString(); dataGridView1[1, i].Value = myReader["name_discip"].ToString(); dataGridView1[2, i].Value = myReader["name_prepod"].ToString(); k = 3;
                        fl = false;
                    }
                    if (k - 3 < col_z)
                        if (myReader["mark"].ToString() == "1")
                        {
                            dataGridView1[k, i].Value = "+"; dataGridView1[k, i].Style.BackColor =
                            System.Drawing.Color.LightGreen;
                        }
                        else
                        if (myReader["mark"].ToString() == "0") dataGridView1[k, i].Value = "-";

                    if (k - 3 >= col_z && k - 3 - col_z < col_l) if (myReader["mark"].ToString() != "n")
                            dataGridView1[k, i].Value = myReader["mark"].ToString();

                    k++;
                    if (k == 3 + col_z + col_l)
                    {
                        i++;
                        fl = true;
                    }
                }
                students_count = i; myReader.Close();
            }

            catch (Exception exp)
            {
                label2.Text = exp.ToString();
            }
        }
        //Метод SqlDataReader.Read - Перемещает SqlDataReader к следующей записи.Метод SqlDataReader.Close - Закрывает объект SqlDataReader.


        //Формирование таблицы задолженностей по лабораторным работам:
private void FillTheTable_Labs()
        {
            string tab = comboBox3.Text; int col = 0;
            SqlDataReader myReader = null;
            SqlCommand myCommand = new SqlCommand("select kol_l from discipline where name_discip='" + tab + "'", myConnection);
            myReader = myCommand.ExecuteReader(); while (myReader.Read())
            {
                col = Convert.ToInt32(myReader["kol_l"]);
            }
            myReader.Close();
            string zadlist = string.Empty; ; for (int i = 0; i < col; i++)
                zadlist += "'l" + (i + 1) + "',"; int index = zadlist.LastIndexOf(','); zadlist = zadlist.Remove(index);

            bool fl = true; int k = 0;
            try
            {
                myReader = null;
                myCommand = new SqlCommand("select mark, name from eregister join discipline on name_ds=discipline.cod join students on name_st=students.nomer " +
                "where name_discip='" + tab + "' and cod_z in (" + zadlist + ")",

                myConnection);


                myReader = myCommand.ExecuteReader(); int i = 0;
                while (myReader.Read())
                {

                    if (fl)
                    {
                        if (!dobavleno_table4) dataGridView4.Rows.Add();
                        dataGridView4.Rows[i].HeaderCell.Value = (i + 1).ToString(); dataGridView4[0, i].Value = myReader["name"].ToString();
                        k = 0;
                        fl = false;
                    }
                    if (myReader["mark"].ToString() == "n") dataGridView4[k + 1, i].Value = false;
                    else
                        dataGridView4[k + 1, i].Value = true; k++;
                    if (k == col)
                    {
                        i++;
                        fl = true;

                    }
                }
                myReader.Close();
            }
            catch (Exception exp)
            {
                label2.Text = exp.ToString();
            }
        }


        //Учёт посещаемости:
string nomer1 = numericUpDown1.Value.ToString(); try
{
for (int i = 0; i<students_count; i++)
{
SqlCommand myCommand = new SqlCommand("update eregister set mark=@z "
+ "where cod_z='z" + nomer1 + "' and name_st=" + (i + 1) + "", myConnection); SqlParameter myParam1 = new SqlParameter("@z", SqlDbType.Char, 1);
if (Convert.ToBoolean(dataGridView2[0, i].Value)) myParam1.Value = '1';
else
myParam1.Value = '0';

myCommand.Parameters.Add(myParam1); myCommand.ExecuteNonQuery();
}
}
catch (Exception exp)
{
label4.Text = exp.ToString();
}


//Подсчет количества пропусков:
if (!dobavleno_pos)
{
dataGridView1.Columns.Add("Pos", "Пропуски"); dobavleno_pos = true;
}
int count = 0;
for (int i = 0; i<students_count; i++)
{
for (int j = 3; j <= 3 + col_l + col_z; j++) if (dataGridView1[j, i].Value == "-")
count++;
dataGridView1["Pos", i].Value = count.ToString(); count = 0;
}


//Определение полномочий преподавателя:
SqlDataReader myReader = null;
SqlCommand myCommand = new SqlCommand("select name_prepod, polnomochiya from teachers", myConnection);
myReader = myCommand.ExecuteReader();
 
int i = 0;
while (myReader.Read())
{
comboBox1.Items.Add(myReader["name_prepod"].ToString()); prepod_mas[i] = myReader["polnomochiya"].ToString(); i++;
}
myReader.Close();



int n = comboBox1.SelectedIndex;
label6.Text = prepod_mas[n];



//Учёт лабораторных работ:
string tab = comboBox3.Text;
int nomer = comboBox2.SelectedIndex; try
{
SqlCommand myCommand;
SqlParameter myParam1 = new SqlParameter("@r1", SqlDbType.Char, 1); SqlParameter myParam4 = new SqlParameter("@nomer", SqlDbType.Int); myParam4.Value = nomer + 1;
for(int i=0;i<col_l;i++)
if (dataGridView3[i, 0].Value != null)
{
myCommand = new SqlCommand("update eregister set mark=@r1 from eregister inner join discipline on name_ds=discipline.cod "
+ "where name_st=@nomer and name_discip='" + tab + "' and cod_z='l"+(i+1)+"'", myConnection);
myParam1.Value = dataGridView3[i, 0].Value; myCommand.Parameters.Add(myParam1); myCommand.Parameters.Add(myParam4); myCommand.ExecuteNonQuery(); myCommand.Parameters.Clear();
}
}
catch (Exception exp)
{
label4.Text = exp.ToString();
}


//Добавление нового предмета:
string name_ds = textBox1.Text;
int count = 0;
SqlDataReader myReader = null;
SqlCommand myCommand = new SqlCommand("select COUNT(*) from discipline", Form1.myConnection);
myReader = myCommand.ExecuteReader(); while (myReader.Read())
{
count=Convert.ToInt32(myReader[""]);
}
myReader.Close();
string newcount = "0" + (count + 1);

myCommand = new SqlCommand("insert discipline (cod, name_discip) values ('"+newcount+"', '"+name_ds+"')", Form1.myConnection);

myCommand.ExecuteNonQuery(); label2.Text = "OK";



Создание журнала по предмету:
int col_z = Convert.ToInt32(numericUpDown1.Value);
int col_l = Convert.ToInt32(numericUpDown2.Value); SqlCommand myCommand;

for (int j = 0; j< 15; j++)
{
for (int i = 0; i<col_z; i++)
{
myCommand = new SqlCommand("insert eregister values ('" + cod_ds + "', 'z"
+ (i + 1) + "', 'n', '" + cod_pr + "', " + (j + 1) + ")", Form1.myConnection); myCommand.ExecuteNonQuery();
}
for (int i = 0; i<col_l; i++)
{
myCommand = new SqlCommand("insert eregister values ('" + cod_ds + "', 'l"
+ (i + 1) + "', 'n', '" + cod_pr + "', " + (j + 1) + ")", Form1.myConnection); myCommand.ExecuteNonQuery();
}
}
myCommand = new SqlCommand("update discipline set dob='1', kol_z="+col_z+", kol_l="+col_l+" where cod='"+cod_ds+"'", Form1.myConnection);
myCommand.ExecuteNonQuery(); label5.Text = "OK";

        }

    }
}
