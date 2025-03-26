using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Storage;


namespace CIS150__Module9YourFirstUWPAPP {
  public sealed partial class MainPage : Page {
    public Student newStudent = new Student();
    public ObservableCollection<Student> studentList = new ObservableCollection<Student>();

    public MainPage() {
      this.InitializeComponent();
    }

    public void SubmitButton_Click(object sender, RoutedEventArgs args) {
      if (newStudent.AnyNull())
        return;

      studentList.Add(new Student(newStudent));
      newStudent.EqualsNew();
    }

    public void CancelButton_Click(object sender, RoutedEventArgs args) {
      if (newStudent.AnyNull())
        return;

      newStudent.EqualsNew();
    }



    //Pops up a file creation menu
    public async void SaveButton_Click(object sender, RoutedEventArgs args) {
      if (studentList.Count <= 0)
        return;

      var picker = new FileSavePicker();
      picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
      picker.FileTypeChoices.Add("Comma Seperated Values", new List<string>() { ".csv" });
      picker.SuggestedFileName = "studentSaves";

      StorageFile file = await picker.PickSaveFileAsync();
      if (file is null)
        return;

      foreach (Student student in studentList) {
        await FileIO.AppendTextAsync(file, student.AsCSV());
      }

      Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
      if (status != Windows.Storage.Provider.FileUpdateStatus.Complete)
        throw new IOException("Could not save file");
    }


    //Deletes all students currently in list
    public void DeleteAllButton_Click(object sender, RoutedEventArgs args) {
      studentList.Clear();
    }


    //Pops up a file selection menu
    public async void OpenButton_Click(object sender, RoutedEventArgs args) {
      var picker = new FileOpenPicker();
      picker.ViewMode = PickerViewMode.List;
      picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
      picker.FileTypeFilter.Add(".csv");

      StorageFile file = await picker.PickSingleFileAsync();
      if (file is null || file.Path is null)
        return;

      studentList.Clear();

      string wholeFile = await FileIO.ReadTextAsync(file);
      string[] lines = wholeFile.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

      for (int i = 0; i < lines.Count(); i++) {
        string line = lines[i];
        if (line is null)
          break;
        if (!Student.TryParse(line, out Student student))
          continue;
        studentList.Add(student);
      }
    }
    

  }
}
