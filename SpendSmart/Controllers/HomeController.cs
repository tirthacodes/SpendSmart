using Microsoft.AspNetCore.Mvc;
using SpendSmart.Models;
using System.Diagnostics;

namespace SpendSmart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly SpendSmartDbContext _context;

        private static decimal totalIncome = 0;


        public HomeController(ILogger<HomeController> logger, SpendSmartDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult AddBudget()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBudget(decimal income)
        {
            var budget = _context.Budgets.FirstOrDefault();

            if (budget == null)
            {
                budget = new Budget { TotalIncome = income }; 
                _context.Budgets.Add(budget); 
            }
            else
            {
                budget.TotalIncome += income;
            }

            _context.SaveChanges();

            ViewBag.TotalIncome = budget.TotalIncome;

            return RedirectToAction("Index");
        }


        public IActionResult Index()
        {
            var allExpenses = _context.Expenses.ToList();

            var totalExpenses = allExpenses.Sum(x => x.value);
            ViewBag.Expenses = totalExpenses;

            var totalIncome = _context.Budgets.SingleOrDefault()?.TotalIncome ?? 0;
            ViewBag.TotalIncome = totalIncome;

            var totalBalance = totalIncome - totalExpenses;
            ViewBag.TotalBalance = totalBalance;



            return View(allExpenses);
        }

        public IActionResult Expenses()
        {
            var allExpenses = _context.Expenses.ToList();

            var totalExpenses = allExpenses.Sum(x => x.value);
            ViewBag.Expenses = totalExpenses;

            return View(allExpenses);
        }

        public IActionResult CreateEditExpenseForm(Expense model)
        {
            if(model.Id == 0)
            {
                //creating something
                _context.Expenses.Add(model);
            }
            else
            {
                //editing something
                _context.Expenses.Update(model);
            }

            _context.SaveChanges();

            return RedirectToAction("Expenses");
        }

        public IActionResult CreateEditExpense(int? id)
        {
            if(id != null)
            {
                //editing an expansy by id
                var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
                return View(expenseInDb);
            }
            
            return View();
        }

        public IActionResult DeleteExpense(int id)
        {
            var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
            _context.Expenses.Remove(expenseInDb);
            _context.SaveChanges();
            return RedirectToAction("Expenses");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
