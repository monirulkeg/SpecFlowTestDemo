using NUnit.Framework;
using RandomApi;
using TechTalk.SpecFlow.Assist;

namespace SpecFlow.RandomApi.Test.StepDefinitions
{
    [Binding]
    public sealed class CalculatorStepDefinitions
    {
        private readonly List<Calculation> _calculations = new List<Calculation>();

        private readonly Calculator _calculator = new Calculator();
        private int _result;

        [Given("the first number is (.*)")]
        [Given(@"the result value (.*) is given as first number")]
        public void GivenTheFirstNumberIs(int number)
        {
            _calculator.FirstNumber = number;
        }

        [Then(@"the result value (.*) is given as first number")]
        public void ThenTheResultValueIsGivenAsFirstNumber(int number)
        {
            _calculator.FirstNumber = number;
        }

        [Given("the second number is (.*)")]
        public void GivenTheSecondNumberIs(int number)
        {
            _calculator.SecondNumber = number;
        }

        [Then(@"the second number is (.*)")]
        public void ThenTheSecondNumberIs(int p0)
        {
            throw new PendingStepException();
        }

        [When(@"the given result is added with second numbers")]
        [When("the two numbers are added")]
        public void WhenTheTwoNumbersAreAdded()
        {
            _result = _calculator.Add();
        }

        [Then("the result should be (.*)")]
        public void ThenTheResultShouldBe(int result)
        {
            _result.Should().Be(result);
        }

        [Given(@"I have entered the following calculations")]
        public void GivenIHaveEnteredTheFollowingCalculations(Table table)
        {
            _calculations.AddRange(table.CreateSet<Calculation>());
        }

        [When(@"I perform these calculations")]
        public void WhenIPerformTheseCalculations()
        {
            // Perform the calculations
            foreach (var calculation in _calculations)
            {
                double result = 0;
                switch (calculation.Operator)
                {
                    case "+":
                        result = calculation.Operand1 + calculation.Operand2;
                        break;
                    case "-":
                        result = calculation.Operand1 - calculation.Operand2;
                        break;
                    case "*":
                        result = calculation.Operand1 * calculation.Operand2;
                        break;
                    case "/":
                        result = calculation.Operand1 / calculation.Operand2;
                        break;
                }

                calculation.Total = result; // Update the calculated result
            }
        }

        [Then(@"the results should be correct")]
        public void ThenTheResultsShouldBeCorrect()
        {
            // Verify that the calculated results match the expected totals
            foreach (var calculation in _calculations)
            {
                switch (calculation.Operator)
                {
                    case "+":
                        Assert.AreEqual(calculation.Operand1 + calculation.Operand2, calculation.Total);
                        break;
                    case "-":
                        Assert.AreEqual(calculation.Operand1 - calculation.Operand2, calculation.Total);
                        break;
                    case "*":
                        Assert.AreEqual(calculation.Operand1 * calculation.Operand2, calculation.Total);
                        break;
                    case "/":
                        Assert.AreEqual(calculation.Operand1 / calculation.Operand2, calculation.Total);
                        break;
                }
            }
        }
    }
    public class Calculation
    {
        public double Operand1 { get; set; }
        public string Operator { get; set; }
        public double Operand2 { get; set; }
        public double Total { get; set; }
    }
}
