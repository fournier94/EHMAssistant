using System;
using System.Windows.Forms;

namespace EHMAssistant
{
    class BirthDateGenerator
    {
        private readonly SecureRandomGenerator _secureRandom;
        private readonly GenerateDraft _draftForm;

        public BirthDateGenerator(GenerateDraft draftForm)
        {
            _secureRandom = new SecureRandomGenerator();
            _draftForm = draftForm;
        }

        public void GenerateBirthDate(Player player)
        {
            // Get the draft year from the form
            if (!TryGetDraftYear(out int draftYear))
            {
                // Default to current year if draft year is invalid
                draftYear = DateTime.Now.Year;
            }

            // For any draft year, players must be born between:
            // October 1st of (draft year - 19) and September 30th of (draft year - 18)
            DateTime startDate = new DateTime(draftYear - 19, 10, 1);
            DateTime endDate = new DateTime(draftYear - 18, 9, 30);

            // Calculate total days in the range
            TimeSpan range = endDate - startDate;
            int totalDays = range.Days;

            // Generate a random number of days to add to the start date
            int randomDays = _secureRandom.GetRandomValue(0, totalDays);
            DateTime birthDate = startDate.AddDays(randomDays);

            // Assign values to player
            player.BirthYear = birthDate.Year.ToString();
            player.BirthMonth = birthDate.Month.ToString();
            player.BirthDay = birthDate.Day.ToString();
        }

        private bool TryGetDraftYear(out int draftYear)
        {
            draftYear = DateTime.Now.Year; // Default value

            // Use reflection to access the private txtDraftYear field
            var textBox = _draftForm.GetType().GetField("txtDraftYear",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance)?.GetValue(_draftForm) as TextBox;

            if (textBox != null && !string.IsNullOrEmpty(textBox.Text))
            {
                if (int.TryParse(textBox.Text, out int year) && year.ToString().Length == 4)
                {
                    draftYear = year;
                    return true;
                }
            }

            return false;
        }

        private int DaysInMonth(int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }
    }
}