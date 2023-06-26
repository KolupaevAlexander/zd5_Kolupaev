using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2
{
    public partial class MainPage : TabbedPage
    {
        double value;
        public MainPage(string userName)
        {
            InitializeComponent();
            datePicker.Date = DateTime.Now;
            helloUser.Text = $"Добро пожаловать, {userName}!";
        }

        private void percentRate_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            value = Math.Round(e.NewValue);
            percent.Text = $"{value}%";
        }

        private void CalculateButton_Clicked(object sender, EventArgs e)
        {
            double sumCredit = 0;
            int termMonths = 0;

            if (!double.TryParse(creditSum.Text, out sumCredit) || !int.TryParse(mounthCount.Text, out termMonths))
            {
                DisplayAlert("Ошибка ввода", "Введите числа в поля \"Срок\" и \"Сумма кредита\"","OK");
                return; 
            }
            string type = typeOfPayment.SelectedItem.ToString();
            double totalAmount;
            double overpayment;
            if (type == "Аннуитетный")
            {
                double monthlyPayment = CalculateAnnuityPayment(sumCredit, termMonths, value);
                totalAmount = monthlyPayment * termMonths;
                overpayment = totalAmount - sumCredit;
                monthPayment.Text = monthlyPayment.ToString();
            }
            else
            {
                totalAmount = CalculateDiffPayment(sumCredit, termMonths, value);
                overpayment = totalAmount - sumCredit;
                monthPayment.Text = "...";
            }
                allSum.Text = Math.Round(totalAmount).ToString();
                overPayment.Text = Math.Round(overpayment).ToString();
        }

        private double CalculateAnnuityPayment(double sumCredit, int termMonths, double value)
        {
            double monthlyRate = value / 100 / 12;
            double denominator = Math.Pow(1 + monthlyRate, termMonths) - 1;
            double annuityPayment = sumCredit * monthlyRate * Math.Pow(1 + monthlyRate, termMonths) / denominator;
            return annuityPayment;
        }

        private double CalculateDiffPayment(double sumCredit, int termMonths, double value)
        {
            double result=0;
            double mainPart= sumCredit/termMonths;
            for (int i=0; i<termMonths; i++) 
            {
                result += mainPart + (sumCredit - mainPart * i) * value / 100 / 12;
            }
            return result;

        }
    }
}
