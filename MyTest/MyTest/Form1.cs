using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        string path; //шлях до файлу
        string name; 

        
        System.Xml.XmlReader xmlReader; // XmlReader для читання текстового файлу

        string quest;     // змінна для питання
        
        
        string[] answer = new string[4];  // масив для відповідей (радіобаттонів)
        string n, g, k, d;// змінні для текст боксів
        string pic;    // шлях до картинки
        int m, s;  // змінні для таймеру
        int right; // номер правильного варіанту відповіді
        int choosequ;   // номер вибраного студентом варіанту відповіді
        int sumrig;     // кількість правильних відповідей
        int sumque;    // загальна кількість питань
        int mode;  // стан програми:
                           // 0 - початок роботи авторизація;
                           // 1 - тестування;
                           // 2 - кінець

        
        public Form1(string[] args)
        {
            InitializeComponent();
            label2.Visible = false; //непотрібні на початку роботи елементи переведені в невидимий режим
            label3.Visible = false;
            label4.Visible = false;
            radioButton1.Visible = false;
            radioButton2.Visible = false;
            radioButton3.Visible = false;
            radioButton4.Visible = false;
            pictureBox1.Visible = false;
            
            if (args.Length > 0)// вказуємо xml файл як параметр команди запуска
            {
              
            if (args[0].IndexOf(":") == -1)  // вказано тільки ім'я файлу
            {
               path = Application.StartupPath + "\\";
               name = args[0];
            }               
            else
            {
                // вказаний шлях
                path = args[0].Substring(0,args[0].LastIndexOf("C:\\Users\\Yaroslav\\Desktop\\Новая папка (2)\\MyTest\\MyTest\\bin\\Debug") +1);
                name = args[0].Substring(args[0].LastIndexOf("questions.xml")+1);
            }          
            xmlReader = new System.Xml.XmlTextReader(path + name);  // отримуємо доступ к xml документу
            xmlReader.Read();

            mode = 0; // режим програми
            sumrig   = 0;           
            this.showHead(); // завантажуємо заголовок                
            this.showDescription(); // та опис
            }
        }

      // функція відображення назви тесту
      private void showHead()
      {
    
        do xmlReader.Read();             // шукаємо <head>
        while(xmlReader.Name != "head");    
        xmlReader.Read();                // читаємо заголовок    
        this.Text = xmlReader.Value;     // виводимо назву в заголовок    
        xmlReader.Read();                // виходимо з  <head>    

      }
   

    // функція для обробки питань
    private void showDescription()
    {

        do                            // шукаємо <description>
        xmlReader.Read();
        while(xmlReader.Name != "description");            
        xmlReader.Read();             // читаємо опис теста            
        label1.Text = xmlReader.Value;// виводимо опис тесту        
        xmlReader.Read();             // виходимо з  <description>  
        do xmlReader.Read();          // шукаємо запитання по тегу <qw>
        while (xmlReader.Name != "qw");        
        xmlReader.Read();            // вмходимо з <qw>
    }

    // читаємо питання
    private Boolean getQw() 
    {
        
        xmlReader.Read();           // шуаємо <q>
        if (xmlReader.Name == "q")
        {
            quest  = xmlReader.GetAttribute("text");  //присвоюємо змінним елементи( питання, картинку)
            pic = xmlReader.GetAttribute("src");
                       
            xmlReader.Read();          // потрапляємо в простір з відповідями
            int i = 0;            
            while (xmlReader.Name != "q")
            {
                xmlReader.Read();              // зчитуємо елементи q <q>                
                if (xmlReader.Name == "a")        // шукаємо за тегом варіанти відповідей
                {
                  if (xmlReader.GetAttribute("answer") == "yes")  // запам'ятовуємо правильну відповідь
                  right = i;
                        
                  xmlReader.Read();          // зчитуємо варіанти відповідей в масив
                  if (i < 4) answer[i] = xmlReader.Value;
                    
                    xmlReader.Read();          // виходимо з <a>
                    i++;
                }
            }            
            xmlReader.Read();      // виходимо з <q>
            return true;
        }
        // на випадок якщо тег не є тегом питання <q>
        else
        return false;
    }   
    private void showQw()  // відображаємо питання та відповідь
    {        
        label1.Text = quest;      // виводимо питання        
        if (pic.Length != 0)// відображення картинок
        {           
           pictureBox1.Image =
           new Bitmap(pic);
           pictureBox1.Visible = true;
           radioButton1.Top = pictureBox1.Bottom + 16;           
        }
        else
        {
          if (pictureBox1.Visible)
              pictureBox1.Visible = false;
              radioButton1.Top = label1.Bottom;
        }

        // відображаємо в кнопках варіанти відповідей
        radioButton1.Text = answer[0];
        radioButton2.Top  = radioButton1.Top + 24;
        radioButton2.Text = answer[1];
        radioButton3.Top  = radioButton2.Top + 24;
        radioButton3.Text = answer[2];
        radioButton4.Top = radioButton3.Top + 24; 
        radioButton4.Text = answer[3];

        radioButton5.Checked = true; 
        button1.Enabled = false;
    }
    
    private void radioButton1_Click(object sender, EventArgs e) // функція бля обробки натиску на кнопки відповідей
    {
        if ((RadioButton)sender == radioButton1) choosequ = 0;
        if ((RadioButton)sender == radioButton2) choosequ = 1;
        if ((RadioButton)sender == radioButton3) choosequ = 2;
        if ((RadioButton)sender == radioButton4) choosequ = 3;
            button1.Enabled = true;
    }
        private void timer1_Tick(object sender, EventArgs e)  //таймер
        {
            
            s = s - 1;
            if (s == -1)//кінець хвилини
            {
                m = m - 1;
                s = 59;
            }

            if (m == 0 && s == 0)//кінець часу
            {
                timer1.Stop();
                MessageBox.Show("Час вийшов!");
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                radioButton3.Visible = false;
                radioButton4.Visible = false;
                pictureBox1.Visible = false;
                if (sumrig == 12)
                    d = "Відмінно ";
                else if (sumrig <= 11 && sumrig >= 10)
                    d = "Дуже добре";
                else if (sumrig <= 9 && sumrig >= 7)
                    d = "Добре";
                else if (sumrig <= 6 && sumrig >= 5)
                    d = "Задовільно";
                else if (sumrig <= 4)
                    d = "Незадовільно";
                label1.Text = "Тестування закінчено.\n" +
                              "Студент " + k.ToString() + " " + "курсу " + " " + "групи " +
                               g.ToString() + " " + n.ToString() + ".\n" +
                               "Правильно відповів на" + " " + sumrig.ToString() + " " +
                               "з" + " " + sumque.ToString() + " " + "питань.\n " +
                               "Ваш рівень знань: " + " " + d.ToString();
            }
            label2.Text = Convert.ToString(m + " :");//відображення часу в програмі
            label3.Text = Convert.ToString(s);

        }
        
       
        private void button1_Click_1(object sender, EventArgs e) // натиск по кнопці ОК
    {
        switch (mode)
        {
        case 0:                         // режим початку роботи
            n = textBox1N.Text;        // присваюємо змінним значення з тестбоксів
            g = textBox2G.Text;
            k = textBox3K.Text;           
            m = 4;                      // хвилини та секунди таймера
            s = 0;
            timer1.Start();
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5N.Visible = false; 
            label6G.Visible = false;
            label7K.Visible = false;
            textBox1N.Visible = false;
            textBox2G.Visible = false;
            textBox3K.Visible = false;
            radioButton1.Visible = true;
            radioButton2.Visible = true;
            radioButton3.Visible = true;
            radioButton4.Visible = true;

            this.getQw();
            this.showQw();

            mode = 1;

            button1.Enabled = false;
            radioButton5.Checked = true;
            break;

        case 1:
            sumque++;   // лічильник кількості питань

            if (choosequ == right) sumrig++; // перевірка на правильність відповіді

            if (this.getQw()) this.showQw();
            else                               // якщо питань більше немає 
            {
                
                timer1.Stop();
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5N.Visible = false;
                label6G.Visible = false;
                label7K.Visible = false;
                textBox1N.Visible = false;
                textBox2G.Visible = false;
                textBox3K.Visible = false;
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                radioButton3.Visible = false;
                radioButton4.Visible = false;
                pictureBox1.Visible  = false;
                if (sumrig == sumque)
                d = "Відмінно ";
                else if (sumrig<=sumque-1 && sumrig>= sumque -2)
                d = "Дуже добре";
                else if (sumrig<=sumque-3 && sumrig>=sumque-5)
                d = "Добре";
                else if (sumrig <= sumque -6 && sumrig >= sumque - 7)
                d = "Задовільно";
                else if (sumrig <= sumque - 8 )
                d = "Незадовільно";

                label1.Text = "Тестування закінчено.\n" +
                              "Студент " + k.ToString() +" "+ "курсу " + " " + "групи "+
                               g.ToString() + " " + n.ToString() + ".\n" +
                               "Правильно відповів на" + " " + sumrig.ToString() + " " + 
                               "з" + " " + sumque.ToString() + " " + "питань.\n "+
                               "Ваш рівень знань: "+" "+d.ToString();
                               
                                        
                              
                        

                // следующий щелчок на кнопке Ok
                // закроет окно программы
                mode = 2;
            }
            break;
            case 2:   // завершение работы программы
            this.Close(); // закрыть окно
            break;
        }
    }


    }
}
