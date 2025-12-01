# Client Contact Model Migration

This document describes the migration from the single `ContactPerson` field to a full `ContactModel` with multiple contacts per client.

## What Changed

### ClientModel
- **Deprecated**: `ContactPerson` property (marked with `[Obsolete]` attribute)
- **Added**: `Contacts` property - a collection of `ContactModel` objects

### New ContactModel
A new model class has been created with the following properties:
- `ClientId` - Reference to the parent client
- `ContactName` - Name of the contact person
- `Title` - Job title
- `Department` - Department name
- `Email` - Contact email
- `PhoneNumber1` - Primary phone number
- `PhoneNumber2` - Secondary phone number
- `MobileNumber` - Mobile phone number
- `IsPrimary` - Indicates if this is the primary contact

### New Repository
- `ContactRepository` - Manages CRUD operations for contacts
- `IContactRepository` - Interface for contact operations

## Usage Examples

### 1. Querying Contacts for a Client

```csharp
using SpectrumV1.DataLayers.Members.Clients;
using SpectrumV1.DataLayers.DataAccess;

// Initialize repository
var contactRepository = new ContactRepository(DatabaseFactory.ProfilePrimary);

// Get all contacts for a specific client
string clientId = "client_id_here";
var contacts = await contactRepository.GetContactsByClientIdAsync(clientId);

// Get primary contact
var primaryContact = contacts.FirstOrDefault(c => c.IsPrimary);
```

### 2. Adding a New Contact

```csharp
var contact = new ContactModel
{
    ClientId = clientId,
    ContactName = "John Doe",
    Title = "Project Manager",
    Department = "Engineering",
    Email = "john.doe@example.com",
    PhoneNumber1 = "+1234567890",
    MobileNumber = "+0987654321",
    IsPrimary = true
};

string newContactId = await contactRepository.AddNewContactAsync(contact);
```

### 3. Updating a Contact

```csharp
var contact = await contactRepository.GetContactByIdAsync(contactId);
contact.Email = "newemail@example.com";
contact.PhoneNumber1 = "+1111111111";

bool updated = await contactRepository.UpdateContactAsync(contact);
```

### 4. Deleting a Contact

```csharp
bool deleted = await contactRepository.DeleteContactAsync(contactId);
```

### 5. Migrating Existing Data

To migrate existing `ContactPerson` data to the new `ContactModel` collection:

```csharp
using SpectrumV1.DataLayers.Members.Clients;

// Create migration helper
var migrationHelper = new ClientContactMigrationHelper(DatabaseFactory.ProfilePrimary);

// Run migration (this is a one-time operation)
await migrationHelper.MigrateContactPersonToContactsAsync();
```

### 6. Syncing Contacts with Client

To populate the `Contacts` collection in `ClientModel`:

```csharp
var migrationHelper = new ClientContactMigrationHelper(DatabaseFactory.ProfilePrimary);
await migrationHelper.SyncContactsToClientAsync(clientId);
```

## Benefits

1. **Multiple Contacts**: Each client can now have multiple contact persons
2. **Structured Data**: Contact information is properly organized with titles, departments, etc.
3. **Primary Contact**: Easy identification of the main contact person
4. **Backward Compatible**: Old `ContactPerson` field is deprecated but not removed
5. **Separate Collection**: Contacts are stored in their own MongoDB collection for better querying
6. **Relational**: Contacts are linked to clients via `ClientId` reference

## Database Structure

### Contacts Collection
```json
{
  "_id": "ObjectId",
  "ClientId": "ObjectId (reference to Clients collection)",
  "ContactName": "string",
  "Title": "string",
  "Department": "string",
  "Email": "string",
  "PhoneNumber1": "string",
  "PhoneNumber2": "string",
  "MobileNumber": "string",
  "IsPrimary": "boolean",
  // ... inherited from EntityObject
  "CreatedBy": "string",
  "CreatedAt": "DateTime",
  "LastModifiedBy": "string",
  "LastModifiedDate": "DateTime",
  "Active": "boolean",
  "Deleted": "boolean"
}
```

### Clients Collection (Updated)
```json
{
  "_id": "ObjectId",
  "ClientName": "string",
  "ContactPerson": "string (deprecated)",
  "Contacts": [
    // Array of ContactModel objects (optional embedded)
  ],
  // ... other client fields
}
```

## Migration Strategy

1. **Phase 1**: Deploy the new models and repositories (completed)
2. **Phase 2**: Run the migration helper to create `ContactModel` records from existing `ContactPerson` data
3. **Phase 3**: Update UI forms to manage contacts collection
4. **Phase 4**: (Optional) Remove the deprecated `ContactPerson` field in a future version

## Notes

- The `ContactPerson` field is marked as `[Obsolete]` but not removed for backward compatibility
- Existing code referencing `ContactPerson` will continue to work with a compiler warning
- The `Contacts` collection in `ClientModel` can be used for embedded display or can be loaded separately
- Consider creating indexes on `ClientId` in the Contacts collection for performance:
  ```javascript
  db.Contacts.createIndex({ "ClientId": 1 })
  ```
