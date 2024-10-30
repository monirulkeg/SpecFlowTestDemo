Feature: Calculator
![Calculator](https://specflow.org/wp-content/uploads/2020/09/calculator.png)
Simple calculator for adding **two** numbers

Link to a feature: [Calculator](SpecFlow.RandomApi.Test/Features/Calculator.feature)
***Further read***: **[Learn more about how to generate Living Documentation](https://docs.specflow.org/projects/specflow-livingdoc/en/latest/LivingDocGenerator/Generating-Documentation.html)**

@mytag
Scenario: Add two numbers
	Given the first number is 50
	And the second number is 70
	When the two numbers are added
	Then the result should be 120
	Given the result value 120 is given as first number
	When the given result is added with second numbers
	Then the result should be 190

Scenario: Performing basic arithmetic operations
    Given I have entered the following calculations
      | Operand1 | Operator | Operand2 | Total |
      | 5.0      | +        | 3.0      | 8.0   |
      | 10.0     | -        | 4.0      | 6.0   |
      | 2.5      | *        | 4.0      | 10.0  |
      | 9.0      | /        | 3.0      | 3.0   |
    When I perform these calculations
    Then the results should be correct

