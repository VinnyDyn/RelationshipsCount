# RelationshipsCount

Display the number of records that are related by entity (1:N) and relationships (N:N).

![alt text](https://github.com/VinnyDyn/RelationshipsCount/blob/master/images/demo-relationshipscount.gif)

### Features
- Click to update data.
- Sort by display name and record's count.
- The data about relationships aren't save.
- Respect the security roles.

### Enable To
- Whole.None
- TwoOptions
- DateAndTime.DateOnly
- DateAndTime.DateAndTime
- Decimal
- FP
- Multiple
- Currency
- OptionSet
- SingleLine.Email
- SingleLine.Text
- SingleLine.TextArea
- SingleLine.URL
- SingleLine.Ticker
- SingleLine.Phone

### Configuration
![alt text](https://github.com/VinnyDyn/RelationshipsCount/blob/master/images/config-relationshipscount.png)
- Attribute-Key: host attribute
- One To Many Entities splitted by comma: optional
- Many To Many Relationships splitted by comma: optional

If the optional parameters haven't value, the entire entity scheme will be considered.

### Developers
After clone the repository, execute the command "npm install" to restore all packages.
