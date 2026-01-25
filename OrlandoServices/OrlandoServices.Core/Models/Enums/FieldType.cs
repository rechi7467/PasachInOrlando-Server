using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.Models.Enums
{
    public enum FieldType
    {

        Text = 1,        // טקסט חופשי
        Number = 2,      // מספר
        Email = 3,       // אימייל
        Phone = 4,       // טלפון
        Date = 5,        // תאריך
        Checkbox = 6,    // כן / לא
        Select = 7,      // רשימה נפתחת
        MultiSelect = 8, // בחירה מרובה
        TextArea = 9     // טקסט ארוך
    }
}
