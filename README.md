# ef-core-sp-multi-result-set
This solution shows how to read multiple results set from a stored procedure called from EF-Core.

This example shows the general way to call an SP and to read/map multiple result sets.

In the SP method caller, you can add Generic types of the model that you want to map. You should pass the SP name and parameters if there are any. This sp is designed to support up to ** 6** different sets.

The stored procedure: 
```
CREATE OR ALTER PROCEDURE usp_getMultiResultSetFromSp
    @id INT = 1,
    @name NVARCHAR(400) = NULL
AS
BEGIN

    SELECT 1 AS Id1,
           'Name1' AS Name1,
           'Description 1' AS Description1;

    SELECT 1 AS Id1,
           'Name2' AS Name2;

    SELECT 1;

    SELECT 1 AS Id1,
           NEWID() AS GeneralId;

END;
```

Caller example1, calls the SP and maps 4 result sets, as this order ResultSet1, ResultSet2, List<int>, and ResultSet4.
```
 var spResultSets = await Repo.ExecuteMultiResultSetSpAsync<ResultSet1, ResultSet2, int, ResultSet4>(
     "usp_getMultiResultSetFromSp", new Dictionary<string, object> { { "id", 1 }, { "name", "admin" } });
 ```

Caller example1, calls the SP and maps 3 result sets, as this order ResultSet1, ResultSet2, and ResultSet4. As you see the 3rd result set is ignored. If we want to ignore a specific result set at a specific position, simply put the **object** as a generic type.
```
var spResultSets = await Repo.ExecuteMultiResultSetSpAsync<ResultSet1, ResultSet2, object, ResultSet4>(
    "usp_getMultiResultSetFromSp", new Dictionary<string, object> { { "id", 1 }, { "name", "admin" } });
```

