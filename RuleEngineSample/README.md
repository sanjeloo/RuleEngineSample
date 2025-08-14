# Microsoft Rules Engine Sample

This project demonstrates how to use Microsoft's Rules Engine library to implement business rules and decision logic in .NET applications.

## Features

The sample includes four different examples:

### 1. Basic Rule Example
- Simple rule evaluation based on input conditions
- Demonstrates basic rule structure and execution

### 2. Business Rules Example
- Loan approval system with multiple rules
- Shows how to implement complex business logic
- Includes credit score, income, and debt-to-income ratio rules

### 3. Dynamic Rule Loading
- Rules loaded from JSON strings
- Demonstrates how to load rules dynamically from external sources
- Useful for database-driven rule management

### 4. Rule Chaining Example
- Rules that depend on each other's outputs
- Shows how to create workflow-like rule execution
- Demonstrates state management between rules

## Prerequisites

- .NET 9.0 SDK
- Visual Studio 2022 or VS Code

## Getting Started

1. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```

2. **Build the project:**
   ```bash
   dotnet build
   ```

3. **Run the sample:**
   ```bash
   dotnet run
   ```

## Dependencies

- **Microsoft.RulesEngine** (v3.0.0) - Core rules engine functionality
- **Newtonsoft.Json** (v13.0.3) - JSON serialization support

## Key Concepts

### Rule Structure
Each rule consists of:
- **RuleName**: Unique identifier for the rule
- **Expression**: Boolean expression that determines if the rule applies
- **Actions**: Dictionary of actions to execute when the rule is satisfied

### Workflow Rules
Rules are organized into workflows:
- **WorkflowName**: Identifier for the workflow
- **Rules**: Collection of rules to execute

### Rule Execution
The engine evaluates rules against input data and returns:
- **IsSuccess**: Whether the rule condition was met
- **RuleEvaluatedParam**: Actions that were executed
- **Rule**: The rule that was evaluated

## Customization

You can easily modify the sample by:
1. Adding new rules to existing workflows
2. Creating new workflows for different business domains
3. Modifying rule expressions to match your business logic
4. Adding custom actions and outputs

## Use Cases

- **Business Process Automation**: Automate decision-making processes
- **Configuration Management**: Dynamic rule-based configuration
- **Validation Systems**: Complex validation logic
- **Workflow Engines**: Business process workflows
- **Policy Management**: Business policy enforcement

## Additional Resources

- [Microsoft Rules Engine Documentation](https://github.com/microsoft/RulesEngine)
- [Rules Engine NuGet Package](https://www.nuget.org/packages/Microsoft.RulesEngine)
- [Expression Language Reference](https://github.com/microsoft/RulesEngine/wiki/Expression-Language)
