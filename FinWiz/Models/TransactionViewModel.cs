using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinWiz.Models
{
    public class TransactionViewModel
    {
        public string Type { get; set; } = "Expense";
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Range(1, 10000000000, ErrorMessage = "Amount should be greater than 0.")] //10ty VNĐ
        public decimal Amount { get; set; }

        [Column(TypeName = "nvarchar(75)")]
        public string? Note { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
        
        public TransactionViewModel() { }
        
        public TransactionViewModel(Expense expense)
        {
            Type = "Expense";
            TransactionId = expense.ExpenseId;
            UserId = expense.UserId;
            CategoryId = expense.CategoryId;
            Category = expense.Category;
            Amount = expense.Amount;
            Note = expense.Note;
            Date = expense.Date;
        }
        
        public TransactionViewModel(Income income)
        {
            Type = "Income";
            TransactionId = income.IncomeId;
            UserId = income.UserId;
            CategoryId = income.CategoryId;
            Category = income.Category;
            Amount = income.Amount;
            Note = income.Note;
            Date = income.Date;
        }

        [NotMapped]
        public string? CategoryTitleWithIcon
        {
            get
            {
                return Category == null ? "" : Category.Icon + " " + Category.Title;
            }
        }

        [NotMapped]
        public string? FormattedAmount
        {
            get
            {
                return ((Category == null || Category.Type == "Expense") ? "- " : "+ ") + Amount.ToString("C0");
            }
        }
    }
}
