namespace UniversalOptimizerTest
{
    public class TestOptimizer
    {

        //// Optimizer can be instantiated with a name, output control, and target problem.
        //[Fact]
        //public void Test_Optimizer_Instantiation()
        //{
        //    // Arrange
        //    string name = "Optimizer";
        //    OutputControl outputControl = new OutputControl();
        //    TargetProblem targetProblem = new FunctionOneVariableProblem(true, "expression", 0.0, 1.0);
        //    //Mock<TargetProblem> targetProblem = new Mock<TargetProblem>();
        //    // Act
        //    Optimizer<object, object> optimizer = new Optimizer<object, object>(name, outputControl, targetProblem);
        //    // Assert
        //    Assert.Equal(name, optimizer.Name);
        //    Assert.Equal(outputControl, optimizer.OutputControl);
        //    Assert.Equal(targetProblem, optimizer.TargetProblem);
        //}

        //// Best solution can be set and retrieved from Optimizer.
        //[Fact]
        //public void Test_Best_Solution_Set_And_Retrieve()
        //{
        //    // Arrange
        //    Optimizer<object, object> optimizer = new Optimizer<object, object>("Optimizer", new OutputControl(), new FunctionOneVariableProblem(true, "expression", 0.0, 1.0));
        //    TargetSolution<object, object> solution = new TargetSolution<object, object>();

        //    // Act
        //    optimizer.BestSolution = solution;
        //    TargetSolution<object, object>? retrievedSolution = optimizer.BestSolution;

        //    // Assert
        //    Assert.Equal(solution, retrievedSolution);
        //}

        //// Execution start and end time can be set and retrieved from Optimizer.
        //[Fact]
        //public void Test_Execution_Start_And_End_Time_Set_And_Retrieve()
        //{
        //    // Arrange
        //    Optimizer<object, object> optimizer = new Optimizer<object, object>("Optimizer", new OutputControl(), new FunctionOneVariableProblem(true, "expression", 0.0, 1.0));
        //    DateTime startTime = DateTime.Now;
        //    DateTime endTime = DateTime.Now.AddSeconds(10);

        //    // Act
        //    optimizer.ExecutionStarted = startTime;
        //    optimizer.ExecutionEnded = endTime;
        //    DateTime retrievedStartTime = optimizer.ExecutionStarted;
        //    DateTime retrievedEndTime = optimizer.ExecutionEnded;

        //    // Assert
        //    Assert.Equal(startTime, retrievedStartTime);
        //    Assert.Equal(endTime, retrievedEndTime);
        //}

        //// Output headers and values can be written to a file.
        //[Fact]
        //public void Test_Write_Output_Headers_And_Values_To_File()
        //{
        //    // Arrange
        //    string filePath = "output.txt";
        //    OutputControl outputControl = new OutputControl();
        //    outputControl.WriteToOutput = true;
        //    outputControl.OutputFile = new StreamWriter(filePath);
        //    Optimizer<object, object> optimizer = new Optimizer<object, object>("Optimizer", outputControl, new FunctionOneVariableProblem(true, "expression", 0.0, 1.0));

        //    // Act
        //    optimizer.WriteOutputHeadersIfNeeded();
        //    optimizer.WriteOutputValuesIfNeeded("afterAlgorithm", "stepNameValue");
        //    outputControl.OutputFile.Close();

        //    // Assert
        //    string[] lines = File.ReadAllLines(filePath);
        //    Assert.Equal(2, lines.Length);
        //}

        //// String representation of Optimizer can be generated.
        //[Fact]
        //public void Test_String_Representation_Generation()
        //{
        //    // Arrange
        //    Optimizer<object, object> optimizer = new Optimizer<object, object>("Optimizer", new OutputControl(), new FunctionOneVariableProblem(true, "expression", 0.0, 1.0));

        //    // Act
        //    string representation = optimizer.StringRep("\n", 0, "");

        //    // Assert
        //    Assert.NotNull(representation);
        //}

        //// Best solution can be set to null.
        //[Fact]
        //public void Test_Best_Solution_Set_To_Null()
        //{
        //    // Arrange
        //    Optimizer<object, object> optimizer = new Optimizer<object, object>("Optimizer", new OutputControl(), new FunctionOneVariableProblem(true, "expression", 0.0, 1.0));
        //    TargetSolution<object, object> solution = new TargetSolution<object, object>();

        //    // Act
        //    optimizer.BestSolution = solution;
        //    optimizer.BestSolution = null;
        //    TargetSolution<object, object>? retrievedSolution = optimizer.BestSolution;

        //    // Assert
        //    Assert.Null(retrievedSolution);
        //}

        //// Output control can be set to null.
        //[Fact]
        //public void Test_Output_Control_Set_To_Null()
        //{
        //    // Arrange
        //    Optimizer<object, object> optimizer = new Optimizer<object, object>("Optimizer", new OutputControl(), new FunctionOneVariableProblem(true, "expression", 0.0, 1.0));

        //    // Act
        //    optimizer.OutputControl = null;
        //    OutputControl retrievedOutputControl = optimizer.OutputControl;

        //    // Assert
        //    Assert.Null(retrievedOutputControl);
        //}

        //// Target problem can be set to null.
        //[Fact]
        //public void Test_Target_Problem_Set_To_Null()
        //{
        //    // Arrange
        //    Optimizer<object, object> optimizer = new Optimizer<object, object>("Optimizer", new OutputControl(), new FunctionOneVariableProblem(true, "expression", 0.0, 1.0));

        //    // Act
        //    optimizer

        //// Execution start and end time can be set to null.
        //[Fact]
        //public void Test_ExecutionTime_Null()
        //    {
        //        // Arrange
        //        Optimizer<object, object> optimizer = new Optimizer<object, object>("Optimizer", new OutputControl(), new FunctionOneVariableProblem(true, "expression", 0.0, 1.0));

        //        // Act
        //        optimizer.ExecutionStarted = null;
        //        optimizer.ExecutionEnded = null;

        //        // Assert
        //        Assert.Null(optimizer.ExecutionStarted);
        //        Assert.Null(optimizer.ExecutionEnded);
        //    }

        //    // Output headers and values can be written to a null file.
        //    [Fact]
        //    public void Test_Output_NullFile()
        //    {
        //        // Arrange
        //        Optimizer<object, object> optimizer = new Optimizer<object, object>("Optimizer", new OutputControl(), new FunctionOneVariableProblem(true, "expression", 0.0, 1.0));

        //        // Act
        //        optimizer.WriteOutputHeadersIfNeeded();
        //        optimizer.WriteOutputValuesIfNeeded("afterAlgorithm", "stepName");

        //        // Assert
        //        // No exception should be thrown
        //    }

        //    // Optimizer can be cloned.
        //    [Fact]
        //    public void Test_Optimizer_Clone()
        //    {
        //        // Arrange
        //        Optimizer<object, object> optimizer = new Optimizer<object, object>("Optimizer", new OutputControl(), new FunctionOneVariableProblem(true, "expression", 0.0, 1.0));

        //        // Act
        //        var clone = optimizer.Clone();

        //        // Assert
        //        Assert.IsType<Optimizer<object, object>>(clone);
        //        Assert.NotSame(optimizer, clone);
        //    }

        //    // Optimizer can be optimized.
        //    [Fact]
        //    public void Test_Optimizer_Optimize()
        //    {
        //        // Arrange
        //        Optimizer<object, object> optimizer = new Optimizer<object, object>("Optimizer", new OutputControl(), new FunctionOneVariableProblem(true, "expression", 0.0, 1.0));

        //        // Act
        //        optimizer.Optimize();

        //        // Assert - Add your assertions here
        //    }

        //    // Name, target problem, and output control can be retrieved from Optimizer.
        //    [Fact]
        //    public void Test_Optimizer_Properties()
        //    {
        //        // Arrange
        //        string name = "Optimizer";
        //        OutputControl outputControl = new OutputControl();
        //        TargetProblem targetProblem = new FunctionOneVariableProblem(true, "expression", 0.0, 1.0);
        //        Optimizer<object, object> optimizer = new Optimizer<object, object>(name, outputControl, targetProblem);

        //        // Act
        //        string optimizerName = optimizer.Name;
        //        OutputControl optimizerOutputControl = optimizer.OutputControl;
        //        TargetProblem optimizerTargetProblem = optimizer.TargetProblem;

        //        // Assert
        //        Assert.Equal(name, optimizerName);
        //        Assert.Equal(outputControl, optimizerOutputControl);
        //        Assert.Equal(targetProblem, optimizerTargetProblem);
        //    }
        }
    }