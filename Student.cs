using System;
using System.ComponentModel;
using System.Linq;

public class Student : INotifyPropertyChanged {
  public event PropertyChangedEventHandler PropertyChanged = delegate { };

  //Allows ID input field to be empty
  public string idAsString {
    get {
      if (ID < 0)
        return "";
      return ID.ToString();
    }
    set {
      if (string.IsNullOrEmpty(value) || !int.TryParse(value, out ID)) ID = -1;
      PropertyChanged.Invoke(this, new PropertyChangedEventArgs("idAsString"));
    }
  }

  //Allows CourseGrade input field to be empty
  public string courseGradeAsString {
    get {
      if (CourseGrade < 0)
        return "";
      return CourseGrade.ToString();
    }
    set {
      if (string.IsNullOrEmpty(value) || !float.TryParse(value, out CourseGrade)) CourseGrade = -1;
      PropertyChanged.Invoke(this, new PropertyChangedEventArgs("courseGradeAsString"));
    }
  }




  public int ID;
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string Course { get; set; }
  public float CourseGrade;




  public void EqualsNew() {
    ID = -1;
    FirstName = "";
    LastName = "";
    Course = "";
    CourseGrade = -1;
    UpdateAll();
  }

  //Updates uwp and regenerates the 'AsString' property
  public void UpdateAll() {
    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(""));
  }

  public bool AnyNull() {
    return ID < 0 || FirstName == "" || LastName == "" || Course == "" || CourseGrade < 0;
  }

  public override string ToString() {
    return $"{ID} - {LastName}, {FirstName} - {Course} {CourseGrade}";
  }

  public string AsCSV() {
    return $"{ID},{FirstName},{LastName},{Course},{CourseGrade}\r\n";
  }


  public static bool TryParse(string value, out Student var) {
    string[] values = value.Split(',', StringSplitOptions.RemoveEmptyEntries);
    var = new Student();

    if (values.Count() != 5)
      return false;

    if (!Int32.TryParse(values[0], out var.ID))
      return false;
    var.FirstName = values[1];
    var.LastName = values[2];
    var.Course = values[3];
    if (!Single.TryParse(values[4], out var.CourseGrade))
      return false;

    var.UpdateAll();
    return true;
  }



  public Student() {
    ID = -1;
    FirstName = "";
    LastName = "";
    Course = "";
    CourseGrade = -1;
    UpdateAll();
  }
  public Student(Student s) {
    ID = s.ID;
    FirstName = s.FirstName;
    LastName = s.LastName;
    Course = s.Course;
    CourseGrade = s.CourseGrade;
    UpdateAll();
  }
  public Student(int i, string fn, string ln, string c, int cg) {
    ID = i;
    FirstName = fn;
    LastName = ln;
    Course = c;
    CourseGrade = cg;
    UpdateAll();
  }
}