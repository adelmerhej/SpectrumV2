# ClientEditForm - Developer Quick Reference

## Common Tasks

### Adding a New Validation Rule

```csharp
// 1. Create a new validation method
private void ValidateYourField(List<string> errors)
{
    if (string.IsNullOrWhiteSpace(txtYourField.Text))
    {
        errors.Add("Your field cannot be empty.");
        if (errors.Count == 1)
        {
            txtYourField.Focus();
        }
    }
}

// 2. Add it to ValidateData()
private bool ValidateData()
{
    var validationErrors = new List<string>();
    
    ValidateClientName(validationErrors);
    ValidateCountry(validationErrors);
    ValidateCity(validationErrors);
    ValidateAddress(validationErrors);
    ValidatePhoneNumber(validationErrors);
    ValidateYourField(validationErrors);  // <-- Add here
    
    if (validationErrors.Any())
    {
        ShowValidationErrors(validationErrors);
        return false;
    }
    return true;
}
```

### Adding a New Repository Dependency

```csharp
// 1. Add field
private readonly YourRepository _yourRepository;

// 2. Initialize in constructor
public ClientEditForm(ClientModel model)
{
    InitializeComponent();
    _clientModel = model ?? new ClientModel();
    
    _clientRepository = new ClientRepository(DatabaseFactory.ProfilePrimary);
    _yourRepository = new YourRepository(DatabaseFactory.ProfilePrimary);  // <-- Add here
    // ... other repositories
    
    StartLoading();
}

// 3. Create loading method
private async Task LoadYourDataAsync()
{
    _yourData = await _yourRepository.GetYourDataAsync();
}

// 4. Add to parallel loading
private async Task InitializeBindings()
{
    var loadTasks = new[]
    {
        LoadCitiesAsync(),
        LoadCountriesAsync(),
        LoadContactsForClientAsync(),
        LoadYourDataAsync()  // <-- Add here
    };
    await Task.WhenAll(loadTasks);
}
```

### Handling New Button Events

```csharp
private void btnYourButton_ItemClick(object sender, ItemClickEventArgs e)
{
    try
    {
        // Your logic here
        
        if (!ValidateSomething())
        {
            return;
        }
        
        DoSomething();
    }
    catch (Exception ex)
    {
        ShowError("Error in your operation", ex);
    }
}
```

### Adding Error-Prone Operations

```csharp
private async Task YourRiskyOperationAsync()
{
    try
    {
        // Your risky code here
        await _repository.DoSomethingAsync();
    }
    catch (Exception ex)
    {
        throw new Exception("Specific error context", ex);
    }
}

// Call it with error handling
private async void btnYourAction_ItemClick(object sender, ItemClickEventArgs e)
{
    try
    {
        await YourRiskyOperationAsync();
    }
    catch (Exception ex)
    {
        ShowError("User-friendly message", ex);
    }
}
```

## Method Templates

### Async Data Loading
```csharp
private async Task Load[EntityName]Async()
{
    _yourCollection = await _repository.Get[EntityName]Async();
}
```

### Safe Selection Retrieval
```csharp
private YourModel GetSelected[EntityName]()
{
    if (!_yourCollection.Any()) return null;
    
    try
    {
        var currentRowIdObj = gvYourGrid.GetFocusedRowCellValue("_id");
        if (currentRowIdObj == null) return null;
        
        string currentRowId = currentRowIdObj.ToString();
        if (string.IsNullOrEmpty(currentRowId)) return null;
        
        return _yourCollection.SingleOrDefault(x => x._id == currentRowId);
    }
    catch (Exception ex)
    {
        ShowError("Error getting selected item", ex);
        return null;
    }
}
```

### Refresh Grid Data
```csharp
private async Task Refresh[EntityName]Async()
{
    await Load[EntityName]Async();
    gc[YourGrid].DataSource = null;
    gc[YourGrid].DataSource = _yourCollection;
    gv[YourGrid].RefreshData();
}
```

### Delete Operation
```csharp
private async void btnDelete[EntityName]_ItemClick(object sender, ItemClickEventArgs e)
{
    if (!CanDelete[EntityName]()) return;
    
    var entity = GetSelected[EntityName]();
    if (entity == null) return;
    
    if (!ConfirmDelete(entity.Name)) return;
    
    try
    {
        entity.Deleted = true;
        await _repository.Delete[EntityName]Async(entity._id);
        await Refresh[EntityName]Async();
    }
    catch (Exception ex)
    {
        HandleDeleteError(ex);
    }
}
```

## Code Patterns

### Pattern: Guard Clauses
```csharp
// Good - Early returns
private void DoSomething()
{
    if (condition1) return;
    if (condition2) return;
    if (condition3) return;
    
    // Main logic here
}

// Avoid - Nested ifs
private void DoSomething()
{
    if (!condition1)
    {
        if (!condition2)
        {
            if (!condition3)
            {
                // Main logic here
            }
        }
    }
}
```

### Pattern: Null-Conditional Operators
```csharp
// Good
SendUpdatedClient?.Invoke(_clientModel, EventArgs.Empty);

// Instead of
if (SendUpdatedClient != null)
{
    SendUpdatedClient.Invoke(_clientModel, EventArgs.Empty);
}
```

### Pattern: Collection Any() Check
```csharp
// Good
if (!_contacts.Any()) return;

// Instead of
if (_contacts == null || _contacts.Count == 0) return;
```

### Pattern: Async Task Naming
```csharp
// Good
private async Task LoadDataAsync() { }
private async Task SaveChangesAsync() { }

// Avoid
private async Task LoadData() { }
private async Task SaveChanges() { }
```

## Common Pitfalls to Avoid

### ? Don't: Catch and Swallow Exceptions
```csharp
try
{
    await _repository.SaveAsync();
}
catch
{
    // Nothing - BAD!
}
```

### ? Do: Handle or Rethrow with Context
```csharp
try
{
    await _repository.SaveAsync();
}
catch (Exception ex)
{
    ShowError("Error saving data", ex);
    // or
    throw new Exception("Context-specific message", ex);
}
```

### ? Don't: Repeat Similar Code
```csharp
// In 5 different methods:
if (string.IsNullOrEmpty(txtField.Text))
{
    XtraMessageBox.Show("Field is required", "Error", ...);
    return false;
}
```

### ? Do: Extract to Reusable Method
```csharp
private bool ValidateRequiredField(TextEdit control, string fieldName, List<string> errors)
{
    if (string.IsNullOrWhiteSpace(control.Text))
    {
        errors.Add($"{fieldName} is required.");
        return false;
    }
    return true;
}
```

### ? Don't: Mix UI and Business Logic
```csharp
private void btnSave_ItemClick(object sender, ItemClickEventArgs e)
{
    if (txtName.Text == "") { ... }
    _model.Name = txtName.Text;
    _repository.Save(_model);
    XtraMessageBox.Show("Saved!");
}
```

### ? Do: Separate Concerns
```csharp
private void btnSave_ItemClick(object sender, ItemClickEventArgs e)
{
    if (ValidateData())
    {
        SaveData();
    }
}

private bool ValidateData() { /* validation logic */ }
private async void SaveData() { /* save logic */ }
```

## Performance Tips

### Parallel Loading
```csharp
// Good - Loads in parallel
await Task.WhenAll(
    LoadCitiesAsync(),
    LoadCountriesAsync(),
    LoadContactsAsync()
);

// Slow - Sequential loading
await LoadCitiesAsync();
await LoadCountriesAsync();
await LoadContactsAsync();
```

### Batch Updates
```csharp
// Good - Update once
gc.DataSource = null;
gc.DataSource = newData;
gv.RefreshData();

// Avoid - Multiple updates in loop
foreach (var item in items)
{
    gv.UpdateCurrentRow();  // Bad in loop!
}
```

## Testing Checklist

Before committing changes:

- [ ] Build successful
- [ ] No compiler warnings
- [ ] All validations work
- [ ] Error messages are user-friendly
- [ ] Null checks in place
- [ ] Async operations properly awaited
- [ ] UI remains responsive
- [ ] Data saves correctly
- [ ] Data loads correctly
- [ ] Delete operations work with confirmation
- [ ] Grid refreshes properly
- [ ] No memory leaks (dispose resources)
- [ ] Exception handling in place

## Debugging Tips

### Check Async Deadlocks
```csharp
// If UI freezes, avoid this:
var result = SomeAsyncMethod().Result;  // BLOCKS!

// Use this instead:
var result = await SomeAsyncMethod();
```

### Check Null References
```csharp
// Add breakpoints and check:
if (_clientModel == null) { /* investigate */ }
if (_contacts == null) { /* investigate */ }
if (gvContacts.GetFocusedRow() == null) { /* investigate */ }
```

### Trace Execution Flow
```csharp
try
{
    System.Diagnostics.Debug.WriteLine("Before operation");
    await Operation();
    System.Diagnostics.Debug.WriteLine("After operation");
}
catch (Exception ex)
{
    System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
    throw;
}
```

## Resources

- See `ClientEditForm_Refactoring_Summary.md` for detailed refactoring explanation
- See `README_ContactMigration.md` for contact model migration guide
- DevExpress documentation: https://docs.devexpress.com/
- C# async/await best practices: https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming
