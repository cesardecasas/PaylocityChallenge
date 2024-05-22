using Api.QueryRepo;
using Api.Models;
using Api.Dtos.Paycheck;

namespace Api.Services
{
    public class PaycheckService
    {

        private readonly DatabaseQueryRepo _databaseService;
        private readonly decimal _baseDeductionAmount;
        private readonly decimal _standardDependantDeduction;
        private readonly decimal _extraDependantDeduction;
        private readonly decimal _extraDeductionForSalaryThreshold;
        private readonly int _salaryThreshold;
        private readonly int _yearlyPaychecks;

        public PaycheckService(DatabaseQueryRepo databaseService)
        {
            _databaseService = databaseService;
            _baseDeductionAmount = 500m;
            _standardDependantDeduction = 300m;
            _extraDeductionForSalaryThreshold = 0.02m;
            _salaryThreshold = 80000;
            _extraDependantDeduction = 100m;
            _yearlyPaychecks = 26;
        }

        public async Task<List<GetPaycheckDto>> GetPaychecksByEmployee(int employeeId)
        {
            var paychecks = new List<Paycheck>();
            var employee = await _databaseService.GetEmployeeById(employeeId);

            if (employee == null || employee.Id == 0) 
            {
                return new List<GetPaycheckDto>();
            }

            var dependents = await _databaseService.GetDependentsByEmployeeId(employeeId);
            var salaryExtraDeductions = new List<Deduction>();
            var dependentDeduction = new Deduction();



            var paycheckSplit = SplitSalaryByPayCheck(employee.Salary);

            if (employee.Salary > _salaryThreshold)
            {
                var deductions = CalculateExtraDeductionBySalaryThreshold(employee.Salary);
                salaryExtraDeductions.AddRange(deductions);
            }

            if (dependents.Count > 0) 
            {
                dependentDeduction = CalculateDependentDeductions(dependents);
            }


            for (int i = 0; i < paycheckSplit.Count; i++)
            {
               
                var paycheck = new Paycheck
                {
                    GrossTotal = paycheckSplit[i],
                    Deductions = new List<Deduction> { 
                        new Deduction
                        {
                            DeductionTotal = _baseDeductionAmount,
                            DeductionType = DeductionTypes.Base
                        } 
                    }
                };

                if(salaryExtraDeductions.Count > 0)
                {
                    paycheck.Deductions.Add(salaryExtraDeductions[i]);
                }

                if(dependentDeduction.DeductionTotal > 0)
                {
                    paycheck.Deductions.Add(dependentDeduction);
                }

                var deductionTotals = paycheck.Deductions.Sum(d => d.DeductionTotal);

                paycheck.NetTotal = paycheck.GrossTotal - deductionTotals;
                paychecks.Add(paycheck);
            }

           
            var paycheckDtos = paychecks.Select(p =>new GetPaycheckDto { GrossTotal = p.GrossTotal, NetTotal = p.NetTotal }).ToList();
            return paycheckDtos;
        }

        public async Task<GetPaycheckDto> GetPaycheckByNumber(int employeeId, int number)
        {
            var paycheckDtos = await GetPaychecksByEmployee(employeeId);
            return paycheckDtos[number - 1];
        }

        private List<decimal> SplitSalaryByPayCheck(decimal salary)
        {
            var parts = _yearlyPaychecks;
            List<decimal> salaryParts = new List<decimal>(new decimal[parts]);

            decimal basePart = Math.Floor(salary / parts * 100) / 100; 
            decimal remainder = salary - basePart * parts; 

            for (int i = 0; i < parts; i++)
            {
                salaryParts[i] = basePart;
            }

            for (int i = 0; i < remainder * 100; i++)
            {
                salaryParts[i] += 0.01m;
            }

            return salaryParts;
        }

        private Deduction CalculateDependentDeductions(List<Dependent> dependents)
        {
            var deduction = new Deduction
            {
                DeductionTotal = 0,
                DeductionType = DeductionTypes.Dependant
            };

            var totalDependantOver50 = CountDependentOver50(dependents);

            if (totalDependantOver50 > 0)
            {
                deduction.DeductionTotal += (dependents.Count * _standardDependantDeduction);
                deduction.DeductionTotal += (totalDependantOver50 * _extraDependantDeduction);
            }
            else
            {
                deduction.DeductionTotal += (dependents.Count * _standardDependantDeduction);
            }

            return deduction;
        }

        private List<Deduction> CalculateExtraDeductionBySalaryThreshold(decimal salary) 
        {
            var extraDeductions = SplitSalaryByPayCheck(salary * _extraDeductionForSalaryThreshold).Select(d => new Deduction { DeductionTotal = d, DeductionType = DeductionTypes.Extra}).ToList();
            return extraDeductions;
        }

        private static int CountDependentOver50(List<Dependent> dependents)
        {
            int count = 0;
            DateTime today = DateTime.Today;

            foreach (var dependent in dependents)
            {
                int age = today.Year - dependent.DateOfBirth.Year;

                if (dependent.DateOfBirth.Date > today.AddYears(-age))
                {
                    age--;
                }

                if (age > 50)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
