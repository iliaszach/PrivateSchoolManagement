using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaProject
{
    class Program
    {
        #region Methods
        private static void SetAssignmentCourseMOD(int StudentID, int CourseID, int assignmentID)
        {
            using (AssignmentPartBEntities db = new AssignmentPartBEntities())
            {
                try
                {
                    var assignmentIDlist = db.StudentAssignments.OrderBy(s => s.AssignmentsCourse.AssignmentID).Where(s => s.AssignmentsCourse.AssignmentID == assignmentID&& s.AssignmentsCourse.CourseID==CourseID && s.StudentCourses.Student.StudentID==StudentID).Select(s => s.AssignmentsCourse.AssignmentID);
                    var assignmentTitle = db.Assignment.OrderBy(s => s.AssignmentID).Where(s => s.AssignmentID == assignmentID).Select(s => s.Title);
                    var CourseIDlist = db.Course.OrderBy(s => s.CourseID).Where(s =>s.CourseID == CourseID).Select(s => s.CourseID);
                    var CourseTitlelist = db.Course.OrderBy(s => s.CourseID).Where(s => s.CourseID == CourseID).Select(s => s.Title);
                    var CourseStreamlist = db.Course.OrderBy(s => s.CourseID).Where(s => s.CourseID == CourseID).Select(s => s.Stream);
                    var studentNamelist = db.StudentCourses.OrderBy(s => s.CourseID).Where(s => s.StudentID == StudentID && s.CourseID == CourseID).Select(s => s.Student.FirstName+" "+s.Student.LastName);

                    int dasda = CourseTitlelist.Count();

                    List<string> findTitle = new List<string>();
                    List<string> findStream = new List<string>();
                    List<string> findName = new List<string>();
                    foreach (var n in CourseTitlelist) 
                    { 
                        findTitle.Add(n); 
                    }
                    foreach (var n in CourseStreamlist) 
                    { 
                        findStream.Add(n); 
                    }
                    
                    string Title = findTitle.Last();
                    string Stream = findStream.Last();

                    foreach (var n in studentNamelist)
                    {
                        findName.Add(n);
                    }
                    string name = findName.Last();
                    string TitleAssignment = String.Empty;
                    List<int> idcheck = new List<int>();
                    List<string> Titlecheck = new List<string>();
                    foreach (var i in assignmentIDlist)
                    {
                        idcheck.Add(i);
                    }                   
                                      
                    foreach (var i in assignmentTitle)
                    {                        
                        Titlecheck.Add(i);
                    }
                    TitleAssignment = Titlecheck.Last();
                    if (idcheck.Count != 0)
                    {
                        Console.WriteLine($"The {TitleAssignment} assignment, for the student {name} and for the course {Title} - {Stream}, is already added");
                    }
                    else
                    {
                        AssignmentsCourse stc = new AssignmentsCourse
                        {
                            AssignmentID = assignmentID,
                            CourseID = CourseID
                        };
                        db.AssignmentsCourse.Add(stc);
                        db.SaveChanges();
                        Console.WriteLine($"The assignment {TitleAssignment} added to the student {name} and to the course {Title} - {Stream}.");


                        var stco = db.StudentCourses.OrderBy(s => s.StudentID).Where(s => s.StudentID == StudentID && s.CourseID == CourseID).Select(s => s.StudentCoursesID);
                        var asco = db.AssignmentsCourse.OrderBy(s => s.AssignmentID).Where(s => s.CourseID == CourseID && s.AssignmentID == assignmentID).Select(s => s.AssignmentsCourseID);
                                                
                        List<int> ascoPK = new List<int>();     

                        foreach (var i in asco)
                        {
                            ascoPK.Add(i);
                        }
                        int AssignmentCourseID = ascoPK.Last();
                        
                        List<int> stcoPK = new List<int>();
                        foreach (var PK in stco)
                        {
                            stcoPK.Add(PK);
                        }
                        int StudentCoursesID = stcoPK.Last();

                        StudentAssignments te = new StudentAssignments()
                        {
                            StudentCoursesID = StudentCoursesID,
                            AssignmentCourseID = AssignmentCourseID
                        };
                        db.StudentAssignments.Add(te);
                        db.SaveChanges();  
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.Source);
                }
            }
        }
        private static void SetCourseSt(int studentID, int courseID)
        {
            using (AssignmentPartBEntities db = new AssignmentPartBEntities())
            {
                try
                {
                    var testlist = from s in db.StudentCourses
                                   orderby s.StudentID
                                   where s.StudentID == studentID
                                   select s;
                    var testlistA = from s in db.Student
                                    orderby s.StudentID
                                    where s.StudentID == studentID
                                    select s;

                    List<string> findname = new List<string>();
                    foreach (var n in testlistA)
                    {
                        findname.Add(n.FirstName + " " + n.LastName);
                    }
                    string name = findname.Last();
                    string Stream = "";
                    List<int> idcheck = new List<int>();
                    int course = courseID;
                    foreach (var i in testlist)
                    {
                        if (i.CourseID == courseID)
                        {
                            idcheck.Add(i.CourseID);
                            Stream = i.Course.Stream;
                            break;
                        }
                    }
                    if (idcheck.Count != 0)
                    {
                        Console.WriteLine($"{name} is already added to the course {Stream}");
                    }
                    else
                    {
                        StudentCourses stc = new StudentCourses
                        {
                            StudentID = studentID,
                            CourseID = courseID
                        };
                        db.StudentCourses.Add(stc);
                        db.SaveChanges();

                        var findSTID = db.StudentCourses.OrderBy(s => s.StudentID).Where(s => s.CourseID == courseID && s.StudentID == studentID).Select(s => s.StudentCoursesID);
                        List<int> giveKey = new List<int>();
                        foreach (var item in findSTID)
                        {
                            giveKey.Add(item);
                        }
                        int StudentCoursesID = giveKey.Last();
                        giveKey.Clear();

                        var findASSCID = db.AssignmentsCourse.OrderBy(s => s.CourseID).Where(s => s.CourseID == courseID).Select(s => s.AssignmentsCourseID);

                        foreach (var item in findASSCID)
                        {
                            giveKey.Add(item);
                        }

                        for (int i = 0; i < findASSCID.Count(); i++)
                        {
                            int AssignmentsCourseIID = giveKey[i];
                            StudentAssignments stass = new StudentAssignments
                            {
                                AssignmentCourseID = AssignmentsCourseIID,
                                StudentCoursesID = StudentCoursesID
                            };
                            db.StudentAssignments.Add(stass);
                            db.SaveChanges();
                        }                                            

                        Console.WriteLine($"{name} is added to the course {Stream}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private static void SetCourseTr(int trainerID, int courseID)
        {
            using (AssignmentPartBEntities db = new AssignmentPartBEntities())
            {
                try
                {
                    var testlist = from s in db.TrainerCourses
                                   orderby s.TrainerID
                                   where s.TrainerID == trainerID
                                   select s;
                    var testlistA = from s in db.Trainer
                                    orderby s.TrainerID
                                    where s.TrainerID == trainerID
                                    select s;
                    List<string> findname = new List<string>();
                    foreach (var n in testlistA)
                    {
                        findname.Add(n.FirstName + " " + n.LastName);
                    }
                    string name = findname.Last();
                    string Stream = "";
                    List<int> idcheck = new List<int>();
                    int course = courseID;
                    foreach (var i in testlist)
                    {
                        if (i.CourseID == courseID)
                        {
                            idcheck.Add(i.CourseID);
                            Stream = i.Course.Stream;
                            break;
                        }
                    }
                    if (idcheck.Count != 0)
                    {
                        Console.WriteLine($"{name} is already added to the course");
                    }
                    else
                    {
                        TrainerCourses stc = new TrainerCourses
                        {
                            TrainerID = trainerID,
                            CourseID = courseID
                        };
                        db.TrainerCourses.Add(stc);
                        db.SaveChanges();
                        Console.WriteLine($"{name} is added to the course {Stream}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        #endregion
        #region Students
        private static void InputStudents()
        {
            Console.Clear();
            Console.WriteLine("Add a student");
            using (AssignmentPartBEntities dbcontext = new AssignmentPartBEntities())
            {
                try
                {
                    bool set = true;
                    while (set)
                    {
                        Console.Write("Enter first name: ");
                        string aFirstName = Console.ReadLine();
                        Console.Write("Enter Last name: ");
                        string aLastName = Console.ReadLine();
                        Console.Write("Enter the date of  birth: ");
                        DateTime aDateOfBirth;
                        var now = new DateTime(2021, 1, 1); 
                        var limit = new DateTime (1903,2, 1);
                        do
                        {
                            DateTime.TryParse(Console.ReadLine(), out aDateOfBirth);
                            if (aDateOfBirth > now || aDateOfBirth < limit) 
                            {
                                Console.WriteLine("Wrong Answer!");
                                Console.WriteLine("Maybe you have to check again your input date of birth.");
                            }
                        } while (aDateOfBirth > now || aDateOfBirth < limit);

                        Console.Write("Enter the tuition fees: ");
                        int aTuitionFees;
                        while (!int.TryParse(Console.ReadLine(), out aTuitionFees))
                        {
                            Console.WriteLine("Wrong Input!");
                            Console.Write("Enter the tuition fees: ");
                        }
                        Student st = new Student()
                        {
                            FirstName = aFirstName,
                            LastName = aLastName,
                            DateOfBirth = aDateOfBirth,
                            TuitionFees = aTuitionFees
                        };
                        dbcontext.Student.Add(st);
                        dbcontext.SaveChanges();

                        var students = dbcontext.Student.OrderBy(s => s.StudentID)                                                     
                                                        .Select(s => s.StudentID);
                        List<int> listKey = new List<int>();
                        foreach (var i in students)
                        {
                            listKey.Add(i);
                        }
                        int StudentID = listKey.Last();
                        Console.WriteLine();
                        Console.WriteLine($"Choose by the index on the left, the course for {aFirstName} {aLastName}: ");

                        int acounter = 1;
                        List<int> cList = new List<int>();
                        var courses = dbcontext.Course.OrderBy(s => s.CourseID).Select(s => s.Title+" "+ s.Stream);
                        var coursesID = dbcontext.Course.OrderBy(s => s.CourseID).Select(s => s.CourseID);

                        foreach (var Courses in courses)
                        {
                            Console.WriteLine($"{acounter}] {Courses}");
                            acounter++;                            
                        }
                        foreach (var courseID in coursesID)
                        {
                            cList.Add(courseID);
                        }
                        int numCourse;
                        do
                        {
                            int.TryParse(Console.ReadLine(), out numCourse);
                             if (numCourse < 1 || numCourse > courses.Count())
                            {
                                Console.WriteLine("Wrong Answer");
                                Console.WriteLine($"You must choose a number between 1 to {courses.Count()}");
                            }
                        } while (numCourse < 1 || numCourse > courses.Count()); 

                        int CourseID = cList[numCourse - 1];

                        StudentCourses stc = new StudentCourses
                        {
                            StudentID = StudentID,
                            CourseID = CourseID
                        };
                        dbcontext.StudentCourses.Add(stc);
                        dbcontext.SaveChanges();
                        var findSTID = dbcontext.StudentCourses.OrderBy(s => s.StudentID).Where(s => s.CourseID == CourseID && s.StudentID == StudentID).Select(s => s.StudentCoursesID);
                        List<int> giveKey = new List<int>();
                        foreach (var item in findSTID)
                        {
                            giveKey.Add(item);
                        }
                        int StudentCoursesID = giveKey.Last();
                        giveKey.Clear();

                        var findASSCID = dbcontext.AssignmentsCourse.OrderBy(s => s.CourseID).Where(s => s.CourseID == CourseID).Select(s => s.AssignmentsCourseID);

                        foreach (var item in findASSCID)
                        {
                            giveKey.Add(item);
                        }                       

                        for (int i = 0; i < findASSCID.Count(); i++)
                        {
                            int AssignmentsCourseID = giveKey[i];
                            StudentAssignments stass = new StudentAssignments
                            {
                                AssignmentCourseID = AssignmentsCourseID,
                                StudentCoursesID = StudentCoursesID
                            };

                            dbcontext.StudentAssignments.Add(stass);
                            dbcontext.SaveChanges();
                        }                       

                        Console.WriteLine($"{aFirstName} {aLastName} is added to the course");

                        Console.WriteLine("Do you want to add another student?");
                        Console.WriteLine("1. Yes \n2. No");
                        var answer = Console.ReadLine();
                        if (answer == "2")
                        {
                            set = false;
                        }
                        Console.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.ReadKey();
        }
        private static void ShowStudents()
        {
            Console.Clear();
            Console.WriteLine("******List of Students******");
            using (AssignmentPartBEntities dbstudents = new AssignmentPartBEntities())
            {
                try
                {
                    var dbStudentPerCourseList = from s in dbstudents.Student
                                                 orderby s.StudentID
                                                 select s;
                    var id = 0;
                    int count = 1;
                    Console.WriteLine($"{"[A/A]", -6}{"Student Name",-25}{"Date of birth",-20}{"Tuition Fees",15}");
                    foreach (var x in dbStudentPerCourseList)
                    {                        
                        Console.WriteLine($"{count+"]",-5} {x.FirstName+" "+x.LastName,-25} {x.DateOfBirth.ToShortDateString(),-15} {Math.Round(x.TuitionFees),15}");
                        count++;
                    }                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.ReadKey();
        }
        #endregion
        #region Trainers
        private static void InputTrainers()
        {
            Console.Clear();
            Console.WriteLine("Add a Trainer");
            using (AssignmentPartBEntities dbcontext = new AssignmentPartBEntities())
            {
                try
                {
                    bool set = true;
                    while (set)
                    {
                        Console.Write("Enter first name: ");
                        string aFirstName = Console.ReadLine();
                        Console.Write("Enter Last name: ");
                        string aLastName = Console.ReadLine();
                        Console.WriteLine("Choose the Subject: ");
                        int acounter = 1;
                        List<int> cList = new List<int>();
                        
                        var courses = dbcontext.Course.OrderBy(s => s.CourseID).Select(s => "Title: " + s.Title + ", Stream: " + s.Stream);
                        var coursesID = dbcontext.Course.OrderBy(s => s.CourseID).Select(s => s.CourseID);
                        
                        foreach (var Courses in courses)
                        {
                            Console.WriteLine($"{acounter}] {Courses}");
                            acounter++;                            
                        }
                        foreach (var CID in coursesID)
                        {
                            cList.Add(CID);
                        }
                        
                        int numCourse;
                        while (!int.TryParse(Console.ReadLine(), out numCourse))
                        {
                            if (numCourse < 1 || numCourse > courses.Count())
                            {
                                Console.WriteLine("Wrong Answer");
                                Console.Write($"You must choose a number between 1 to {courses.Count()}");
                            }
                        }
                        Console.WriteLine();
                        int CourseID = cList.Last();
                        List<string> cListA = new List<string>();
                        var coursesStream = dbcontext.Course.OrderBy(s => s.CourseID).Where(s => s.CourseID == CourseID).Select(s => s.Stream);
                        foreach (var i in coursesStream)
                        { 
                            cListA.Add(i);                            
                        }
                        string aSubject = cListA[0];
                        Trainer st = new Trainer()
                        {
                            FirstName = aFirstName,
                            LastName = aLastName,
                            Subject = aSubject
                        };
                        dbcontext.Trainer.Add(st);
                        dbcontext.SaveChanges();
            
                        var trainers = dbcontext.Trainer.OrderBy(s => s.TrainerID).Select(s => s.TrainerID);

                        List<int> listKey = new List<int>();
                        foreach (var i in trainers)
                        {
                            listKey.Add(i);
                        }
                        int TrainerID = listKey.Last();

                        TrainerCourses stc = new TrainerCourses
                        {
                            TrainerID = TrainerID,
                            CourseID = CourseID
                        };
                        dbcontext.TrainerCourses.Add(stc);
                        dbcontext.SaveChanges();
                        Console.WriteLine($"{aFirstName} {aLastName} is added to the course");
                        Console.WriteLine("Do you want to add another Trainer?");
                        Console.WriteLine("1. Yes \n2. No");
                        var answer = Console.ReadLine();
                        if (answer == "2")
                        {
                            set = false;
                        }
                        Console.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.ReadKey();
        }
        private static void ShowTrainers()
        {
            Console.Clear();
            Console.WriteLine("List of Trainers");
            using (AssignmentPartBEntities dbtraines = new AssignmentPartBEntities())
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("*****List of Trainers*****");
                    using (AssignmentPartBEntities db = new AssignmentPartBEntities())
                    {
                        try
                        {
                            var dbTrainersPerCourseList = from trainer in db.Trainer
                                                          orderby trainer.TrainerID
                                                          select trainer;
                            
                            int count = 1;
                            Console.WriteLine($"{"[A/A]",-6}{"Trainer Name",-25}{"Subject",-20}");
                            foreach (var x in dbTrainersPerCourseList)
                            {
                                Console.WriteLine($"{count + "]",-5} {x.FirstName +" "+ x.LastName,-25} {x.Subject,-15}");
                                count++;
                            }                            
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.ReadKey();
        }
        #endregion
        #region Courses
        private static void InputCourses()
        {
            Console.Clear();
            Console.WriteLine("Add a Course");
            using (AssignmentPartBEntities dbcontext = new AssignmentPartBEntities())
            {
                try
                {
                    bool set = true;
                    while (set)
                    {
                        Console.Write("Enter the title: ");
                        string aTitle = Console.ReadLine();
                        Console.Write("Enter the stream: ");
                        string aStream = Console.ReadLine();
                        Console.Write("Enter the type: ");
                        string aType = Console.ReadLine();
                        Console.Write("Enter the start date: ");
                        DateTime aStartDate;
                        var apo = new DateTime(2021, 1, 1);
                        var mexri = new DateTime(2021, 2, 1);
                        do
                        {
                            DateTime.TryParse(Console.ReadLine(), out aStartDate);
                            if (aStartDate < apo || aStartDate > mexri)
                            {
                                Console.WriteLine("Wrong Answer!");
                                Console.WriteLine("The start-period is between 1/1/2021-1/2/2021.");
                            }
                        } while (aStartDate < apo || aStartDate > mexri);

                        
                        Console.Write("Enter the end date: ");
                        DateTime aEndDate;
                        var apo_ = new DateTime(2021, 12, 1);
                        var mexri_ = new DateTime(2021, 12, 31);
                        do
                        {
                            DateTime.TryParse(Console.ReadLine(), out aEndDate);
                            if (aEndDate < apo_ || aEndDate > mexri_)
                            {
                                Console.WriteLine("Wrong Answer!");
                                Console.WriteLine("The end-period is between 1/12/2021-31/12/2021.");
                            }
                        } while (aEndDate < apo_ || aEndDate > mexri_);

                        Course st = new Course()
                        {
                            Title = aTitle,
                            Stream = aStream,
                            Type = aType,
                            Start_Date = aStartDate,
                            End_Date = aEndDate
                        };
                        dbcontext.Course.Add(st);
                        dbcontext.SaveChanges();
                        var courses = dbcontext.Course.OrderBy(s => s.CourseID).Select(s => s.Title + " - " + s.Stream);
                        var coursesID = dbcontext.Course.OrderBy(s => s.CourseID).Select(s => s.CourseID);
                        List<int> listKey = new List<int>();
                        foreach (var i in coursesID)
                        {
                            listKey.Add(i);
                        }
                        int CourseID = listKey.Last();
                        Console.WriteLine();
                        Console.WriteLine($"Choose by the index on the left, the Assignment for the course {aTitle} - {aStream}: ");
                        int acounter = 1;
                        List<int> cList = new List<int>();                        
                        var assignments = dbcontext.Assignment.OrderBy(s => s.AssignmentID).Select(s => "Title: " + s.Title+ ", Stream: "+s.Description);
                        var assignmentsID = dbcontext.Assignment.OrderBy(s => s.AssignmentID).Select(s => s.AssignmentID);
                        foreach (var asg in assignments)
                        {
                            Console.WriteLine($"{acounter}] {asg}");
                            acounter++;                            
                        }
                        foreach (var A_ID in assignmentsID)
                        {
                            cList.Add(A_ID);
                        }
                        int choice;                        
                        do
                        {
                            int.TryParse(Console.ReadLine(), out choice);
                            if (choice < 1 || choice > assignments.Count())
                            {
                                Console.WriteLine("Wrong Answer");
                                Console.WriteLine($"You must choose a number between 1 to {assignments.Count()}");
                            }
                        } while (choice < 1 || choice > assignments.Count());

                        int AssignmentID = cList[choice - 1];
                        
                        AssignmentsCourse stc = new AssignmentsCourse
                        {
                            AssignmentID = AssignmentID,
                            CourseID = CourseID
                        };
                        dbcontext.AssignmentsCourse.Add(stc);
                        dbcontext.SaveChanges();
                        Console.WriteLine($"Assigment is added to the course");
                        Console.WriteLine();
                        Console.WriteLine("Do you want to add another Course?");
                        Console.WriteLine("1. Yes \n2. No");
                        var answer = Console.ReadLine();
                        if (answer == "2")
                        {
                            set = false;
                        }
                        Console.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.ReadKey();
        }
        private static void ShowCourses()
        {
            Console.Clear();
            Console.WriteLine("*****List of Courses*****");
            using (AssignmentPartBEntities dbCourses = new AssignmentPartBEntities())
            {
                try
                {
                    var dbCourseslist = from c in dbCourses.Course
                                        orderby c.CourseID
                                        select c;
                    int count = 1;
                    Console.WriteLine($"{"[A/A]",-6}{"Title",-15}{"Stream",-18}{"Type",-12}{"Date of Start",-18}{"Date of End",-15}");
                    foreach (var x in dbCourseslist)
                    {
                        Console.WriteLine($"{count + "]",-5} {x.Title,-15} {x.Stream,-15}{x.Type,-15} {x.Start_Date.ToShortDateString(),-15} {x.End_Date.ToShortDateString(),-15}");
                        count++;
                    }                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.ReadKey();
        }
        #endregion
        #region Assignments
        private static void InputAssignments()
        {
            Console.Clear();
            Console.WriteLine("Add an Assignment");
            using (AssignmentPartBEntities dbcontext = new AssignmentPartBEntities())
            {
                try
                {
                    bool set = true;
                    while (set)
                    {
                        Console.Write("Enter the title: ");
                        string aTitle = Console.ReadLine();
                        Console.Write("Enter the description: ");
                        string aDescription = Console.ReadLine();
                        Console.Write("Enter the substitution date: ");
                        DateTime aSubDate;                        
                        var apo_ = new DateTime(2021, 1, 1);
                        var mexri_ = new DateTime(2021, 12, 31);
                        do
                        {
                            DateTime.TryParse(Console.ReadLine(), out aSubDate);
                            if (aSubDate < apo_ || aSubDate > mexri_)
                            {
                                Console.WriteLine("Wrong Answer!");
                                Console.WriteLine("The end-period is between 1/1/2021-31/12/2021.");
                            }
                        } while (aSubDate < apo_ || aSubDate > mexri_);
                        Console.Write("Enter the oral mark: ");
                        int aOralMark;                        
                        do
                        {
                            int.TryParse(Console.ReadLine(), out aOralMark);
                            if (aOralMark < 1 || aOralMark > 50)
                            {
                                Console.WriteLine("Wrong Answer, the lowest oral mark is -0- and the highest is -50-");
                                Console.Write("Enter the oral mark: ");
                            }
                        } while (aOralMark < 1 || aOralMark > 50);
                        Console.Write("Enter the total mark: ");
                        int aTotalMark; 
                        do
                        {
                            int.TryParse(Console.ReadLine(), out aTotalMark);
                            if (aTotalMark < 1 || aTotalMark > 50)
                            {
                                Console.WriteLine("Wrong Answer, the lowest oral mark is -0- and the highest is -50-");
                                Console.Write("Enter the oral mark: ");
                            }
                        } while (aTotalMark < 1 || aTotalMark > 50);



                        Assignment st = new Assignment()
                        {
                            Title = aTitle,
                            Description = aDescription,
                            SubDateTime = aSubDate,
                            OralMark = aOralMark,
                            TotalMark = aTotalMark
                        };
                        dbcontext.Assignment.Add(st);
                        dbcontext.SaveChanges();
                        //-----------------------------------------------
                        var findAsID = dbcontext.Assignment.OrderBy(s => s.AssignmentID).Select(s => s.AssignmentID);
                        List<int> giveKey = new List<int>();
                        foreach (var item in findAsID)
                        {
                            giveKey.Add(item);
                        }
                        int AssignmentID = giveKey.Last();
                        giveKey.Clear();
                        Console.WriteLine($"Choose by the index on the left, the course for assignment {aTitle} : {aDescription}: ");

                        int acounter = 1;
                        List<int> cList = new List<int>();
                        List<string> cListA = new List<string>();
                        var courses = dbcontext.Course.OrderBy(s => s.CourseID).Select(s => s.Title + " " + s.Stream);
                        var coursesID = dbcontext.Course.OrderBy(s => s.CourseID).Select(s => s.CourseID);

                        foreach (var Courses in courses)
                        {
                            Console.WriteLine($"{acounter}] {Courses}");
                            acounter++;
                            cListA.Add(Courses);
                        }
                        foreach (var courseID in coursesID)
                        {
                            cList.Add(courseID);
                        }
                        
                        int choice;
                        do
                        {
                            int.TryParse(Console.ReadLine(), out choice);
                            if (choice < 1 || choice > courses.Count())
                            {
                                Console.WriteLine("Wrong Answer");
                                Console.WriteLine($"You must choose a number between 1 to {courses.Count()}");
                            }
                        } while (choice < 1 || choice > courses.Count());
                        string CourseTitle =cListA[choice - 1];
                        
                        int CourseID = cList[choice - 1];

                        AssignmentsCourse asc = new AssignmentsCourse
                        {
                            AssignmentID = AssignmentID,
                            CourseID = CourseID
                        };
                        dbcontext.AssignmentsCourse.Add(asc);
                        dbcontext.SaveChanges();
                        
                        Console.WriteLine($"The assignment {aTitle} : {aDescription} is added to the course {CourseTitle}");

                        Console.WriteLine("\nDo you want to add another Assignment?");
                        Console.WriteLine("1. Yes \n2. No");
                        var answer = Console.ReadLine();
                        if (answer == "2")
                        {
                            set = false;
                        }
                        Console.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.ReadKey();
        }
        private static void ShowAssignments()
        {
            Console.Clear();
            Console.WriteLine("*****List of Assignments*****");
            using (AssignmentPartBEntities dbAssignments = new AssignmentPartBEntities())
            {
                try
                {
                    var dbAssignmentslist = from s in dbAssignments.Assignment orderby s.AssignmentID select s;
                        int count = 1;
                    Console.WriteLine($"{"[A/A]",-6}{"Title",-18}{"Description",-16}{"Oral Mark",-12}{"Total Mark",-15}{"Date of Substitution",-15}");
                    foreach (var x in dbAssignmentslist)
                    {
                        Console.WriteLine($"{count + "]",-5} {x.Title,-15} {x.Description,-20}{x.OralMark,-12}{x.TotalMark,-12} {x.SubDateTime,-15} ");
                        count++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.ReadKey();
        }

        #endregion
        #region Per courses
        public static void ShowTrainersPerCourse()
        {
            Console.Clear();
            Console.WriteLine("*****List of all Trainers per Course*****");
            using (AssignmentPartBEntities db = new AssignmentPartBEntities())
            {
                try
                {
                    var dbTrainersPerCourseList = from trainerCourses in db.TrainerCourses
                                                  orderby trainerCourses.CourseID
                                                  select trainerCourses;
                    var count = 1;
                    Console.WriteLine($"{"[A/A]",-6}{"Title of Course",-24}{"Name of Trainer",-25}");
                    foreach (var x in dbTrainersPerCourseList)
                    {
                        Console.WriteLine($"{count + "]",-8} {x.Course.Title+" - "+x.Course.Stream,-18} {x.Trainer.FirstName + " " + x.Trainer.LastName,-25}");
                        count++;
                    }  
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.ReadKey();
        }
        private static void SetAssignmentPerStudentPerCourse()
        {
            Console.Clear();
            Console.WriteLine("Set Assignments per Student per Course");
            using (AssignmentPartBEntities dbcontext = new AssignmentPartBEntities())
            {
                try
                {
                    var students = from s in dbcontext.Student
                                   orderby s.StudentID
                                   select s;
                    Console.WriteLine("Choose, by the index on the left, the student you want to modifie the course choice");
                    int count = 1;
                    List<int> stList = new List<int>();
                    foreach (var student in students)
                    {
                        Console.WriteLine($"{count}] {student.FirstName} {student.LastName}");
                        count++;
                        stList.Add(student.StudentID);
                    }
                    int number;                    
                    do
                    {
                        int.TryParse(Console.ReadLine(), out number);
                        if (number < 1 || number > students.Count())
                        {
                            Console.WriteLine("Wrong Answer");
                            Console.WriteLine($"You must choose a number between 1 to {students.Count()}");
                        }
                    } while (number < 1 || number > students.Count());

                    int StudentID = stList[number - 1];
                    var stname = from s in students
                                 where s.StudentID == StudentID
                                 select s;
                    foreach (var student in stname)
                    {
                        Console.WriteLine($"You chose the student {student.FirstName} {student.LastName}");
                    }
                    var coursesStream = dbcontext.StudentCourses.OrderBy(s => s.StudentID).Where(s => s.StudentID == StudentID).Select(s => s.Course.Stream);                  
                      
                    
                    Console.WriteLine("\n{Student} has the above courses.\nChoose, by the index on the left, the course : ");
                    int acounter = 1;
                    
                    List<int> cList = new List<int>();
                    List<int> cListID = new List<int>();
                    foreach (var Courses in coursesStream)
                    {
                        Console.WriteLine($"{acounter}] Stream: {Courses}");
                        acounter++;                        
                    }  
                    int numCourse;
                    do
                    {
                        int.TryParse(Console.ReadLine(), out numCourse);
                        if (numCourse < 1 || numCourse > coursesStream.Count())
                        {
                            Console.WriteLine("Wrong Answer");
                            Console.WriteLine($"You must choose a number between 1 to {coursesStream.Count()}");
                        }
                    } while (numCourse < 1 || numCourse > coursesStream.Count());

                    var coursesID = dbcontext.StudentCourses.OrderBy(s => s.CourseID).Where(s => s.StudentID == StudentID).Select(s => s.CourseID);
                    foreach (var item in coursesID)
                    {
                        cListID.Add(item);
                    }
                    int CourseID = cListID[numCourse - 1];
                    List<string> title = new List<string>();
                    List<string> stream = new List<string>();
                    var cID = from s in dbcontext.Course
                              orderby s.CourseID
                              where s.CourseID == CourseID
                              select s;
                    foreach (var x in cID)
                    {
                        title.Add(x.Title);
                        stream.Add(x.Stream);
                    }
                    string aTitleB = title[0];
                    string aStreamB = stream[0];                    
                    Console.WriteLine($"Choose by the index on the left, the assignment for the course {aTitleB} - {aStreamB}: ");
                    int acount = 1;
                    List<int> cListA = new List<int>();
                    var assignments = from s in dbcontext.Assignment
                                      orderby s.AssignmentID
                                      select s;
                    foreach (var asg in assignments)
                    {
                        Console.WriteLine($"{acount}] Title: {asg.Title}, Description: {asg.Description}");
                        acount++;
                        cListA.Add(asg.AssignmentID);
                    }
                    int numAss;
                    do
                    {
                        int.TryParse(Console.ReadLine(), out numAss);
                        if (numAss < 1 || numAss > assignments.Count())
                        {
                            Console.WriteLine("Wrong Answer");
                            Console.WriteLine($"You must choose a number between 1 to {assignments.Count()}");
                        }
                    } while (numAss < 1 || numAss > assignments.Count());

                    int AssignmentID = cListA[numAss - 1];
                    SetAssignmentCourseMOD(StudentID, CourseID, AssignmentID);
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.Source);
                    Console.WriteLine(ex.HelpLink);
                    Console.WriteLine(ex.Data);
                }
            }
            Console.ReadKey();
        }
        private static void SetStudentPerCourse()
        {
            Console.Clear();
            Console.WriteLine("Set Students per Course");
            using (AssignmentPartBEntities dbcontext = new AssignmentPartBEntities())
            {
                try
                {
                    var students = from s in dbcontext.Student
                                   orderby s.StudentID
                                   select s;
                    Console.WriteLine("Choose, by the index on the left, the student you want to modifie the course choice");
                    int count = 1;
                    List<int> stList = new List<int>();
                    foreach (var student in students)
                    {
                        Console.WriteLine($"{count}] {student.FirstName} {student.LastName}");
                        count++;
                        stList.Add(student.StudentID);
                    }
                    
                    int num;
                    do
                    {
                        int.TryParse(Console.ReadLine(), out num);
                        if (num < 1 || num > students.Count())
                        {
                            Console.WriteLine("Wrong Answer");
                            Console.WriteLine($"You must choose a number between 1 to {students.Count()}");
                        }
                    } while (num < 1 || num > students.Count());

                    int StudentID = stList[num - 1];
                    var stname = from s in students
                                 where s.StudentID == StudentID
                                 select s;
                    foreach (var student in stname)
                    {
                        Console.WriteLine($"You chose the student {student.FirstName} {student.LastName}");
                    }
                    var courses = from s in dbcontext.Course
                                  orderby s.CourseID
                                  select s;
                    Console.WriteLine("Choose, by the index on the left, the course");

                    int acounter = 1;
                    List<int> cList = new List<int>();

                    foreach (var Courses in courses)
                    {
                        Console.WriteLine($"{acounter}] Title: {Courses.Title}, Stream: {Courses.Stream}");
                        acounter++;
                        cList.Add(Courses.CourseID);
                    }
                    
                    int numCourse;
                    do
                    {
                        int.TryParse(Console.ReadLine(), out numCourse);
                        if (numCourse < 1 || numCourse > courses.Count())
                        {
                            Console.WriteLine("Wrong Answer");
                            Console.WriteLine($"You must enter a number between 1 and {courses.Count()}");
                        }
                    } while (numCourse < 1 || numCourse > courses.Count());

                    int CourseID = cList[numCourse - 1];
                    SetCourseSt(StudentID, CourseID);
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private static void SetTrainerPerCourse()
        {
            Console.Clear();
            Console.WriteLine("Set Trainers per Course");
            using (AssignmentPartBEntities dbcontext = new AssignmentPartBEntities())
            {
                try
                {
                    var trainers = from s in dbcontext.Trainer
                                   orderby s.TrainerID
                                   select s;
                    Console.WriteLine("Choose, by the index on the left, the trainer you want to modifie the course choice");
                    int count = 1;
                    List<int> stList = new List<int>();
                    foreach (var t in trainers)
                    {
                        Console.WriteLine($"{count}] {t.FirstName} {t.LastName}");
                        count++;
                        stList.Add(t.TrainerID);
                    }
                    int num;                    
                    do
                    {
                        int.TryParse(Console.ReadLine(), out num);
                        if (num < 1 && num > trainers.Count())
                        {
                            Console.WriteLine("Wrong Answer");
                            Console.WriteLine($"You must enter a number between 1 and {trainers.Count()}");
                        }
                    } while (num < 1 && num > trainers.Count());


                    int TrainerID = stList[num - 1];
                    var stname = from s in trainers
                                 where s.TrainerID == TrainerID
                                 select s;
                    foreach (var t in stname)
                    {
                        Console.WriteLine($"You chose the trainer {t.FirstName} {t.LastName}");
                    }
                    var courses = from s in dbcontext.Course
                                  orderby s.CourseID
                                  select s;
                    Console.WriteLine("Choose, by the index on the left, the course");

                    int acounter = 1;
                    List<int> cList = new List<int>();

                    foreach (var Courses in courses)
                    {
                        Console.WriteLine($"{acounter}] Title: {Courses.Title}, Stream: {Courses.Stream}");
                        acounter++;
                        cList.Add(Courses.CourseID);
                    }
                    int numCourse;                  
                    do
                    {
                        int.TryParse(Console.ReadLine(), out numCourse);
                        if (numCourse < 1 || numCourse > courses.Count())
                        {
                            Console.WriteLine("Wrong Answer");
                            Console.WriteLine($"You must enter a number between 1 and {courses.Count()}");
                        }
                    } while (numCourse < 1 || numCourse > courses.Count());

                    int CourseID = cList[numCourse - 1];
                    SetCourseTr(TrainerID, CourseID);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.ReadKey();
        }
        public static void ShowStudentsPerCourse()
        {
            Console.Clear();
            Console.WriteLine("*****List of Students per Course*****");
            using (AssignmentPartBEntities db = new AssignmentPartBEntities())
            {
                try
                {
                    var dbStudentPerCourseList = from s in db.StudentCourses
                                                 orderby s.CourseID
                                                 select s;
                    
                    int count = 1;
                    Console.WriteLine($"{"[A/A]",-6}{"Title of Course",-24}{"Name of Student",-25}");
                    foreach (var x in dbStudentPerCourseList)
                    {
                        Console.WriteLine($"{count + "]",-8} {x.Course.Title + " - " + x.Course.Stream,-18} {x.Student.FirstName + " " + x.Student.LastName,-25}");
                        count++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            Console.ReadKey();
        }
        public static void ShowStudentsPerCourseMoreThanOneCourse()
        {
            Console.Clear();
            Console.WriteLine("*****List of Students with more than one Course*****");
            using (AssignmentPartBEntities db = new AssignmentPartBEntities())
            {
                try
                {
                    var results = from studentCourses in db.StudentCourses
                                  join student in db.Student
                                  on studentCourses.StudentID equals student.StudentID
                                  join course in db.Course
                                  on studentCourses.CourseID equals course.CourseID
                                  let stName = student.FirstName + " " + student.LastName
                                  group course.Stream by student into g
                                  let NumCourses = g.Count()
                                  where NumCourses > 1
                                  orderby NumCourses descending
                                  select new
                                  { stName = g.Key.FirstName + " " + g.Key.LastName, NumCourses };


                    Console.WriteLine($"{"[A/A]",-6}{"Name of Student",-25}{"Number of Courses",-24}");
                    var count = 1;
                    foreach (var item in results)
                    {
                        Console.WriteLine($"{count+"]",-7} {item.stName,-30}  {item.NumCourses,-24}");
                        count++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            Console.ReadKey();
        }
        public static void ShowAssignmentsPerCourse()
        {
            Console.Clear();
            Console.WriteLine("*****List of Assignments per Course*****");
            using (AssignmentPartBEntities db = new AssignmentPartBEntities())
            {
                try
                {
                    var dbAssignmentsCourseList = from assignmentsCourse in db.AssignmentsCourse
                                                  orderby assignmentsCourse.CourseID
                                                  select assignmentsCourse;
                    
                    int count = 1;
                    Console.WriteLine($"{"[A/A]",-6}{"Title of Assignment",-25}{"Title of Course",-24}");
                    foreach (var x in dbAssignmentsCourseList)
                    {
                        Console.WriteLine($"{count+"]",-8} {x.Assignment.Title, -25} {x.Course.Title+" - "+x.Course.Stream,-10}");
                        count++;   
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.ReadKey();
        }
        public static void ShowAssignmentsPerCoursePerStudent()
        {
            Console.Clear();
            Console.WriteLine("*****List of Assignments per Course per Students*****");
            using (AssignmentPartBEntities db = new AssignmentPartBEntities())
            {
                try
                {
                    var dbstudentAssignmentsList = from s in db.StudentAssignments
                                                   orderby s.StudentCourses.StudentID, s.StudentCourses.CourseID
                                                   select s;                    
                    int count = 1;
                    Console.WriteLine($"{"A/A",-6} {"Name of Student",-22} {"Course",-18} {"Assignment",-10}");
                    foreach (var x in dbstudentAssignmentsList)
                    {
                        Console.WriteLine($"{count + "]",-8}{x.StudentCourses.Student.FirstName + " " + x.StudentCourses.Student.LastName,-20} {x.StudentCourses.Course.Title + " - " + x.StudentCourses.Course.Stream,-20}{x.AssignmentsCourse.Assignment.Title,-10}");
                        count++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.ReadKey();
        }
        #endregion
        #region Menu Methods
        private static bool MainMenu()
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.WriteLine("******************This project belongs to Zacharioudakis Ilias and the science community********************");
            Console.WriteLine("Welcome to our School!!!");
            Console.WriteLine("Choose an option");
            Console.WriteLine("1) Input Data");
            Console.WriteLine("2) List Menu");
            Console.WriteLine("3) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    TheInputDataMenu();
                    return true;
                case "2":
                    TheListMenu();
                    return true;
                case "3":
                    return false;
                default:
                    return true;
            }
        }

        private static void TheListMenu()
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = ExteListMenu();
            }
        }

        private static bool ExteListMenu()
        {
            Console.Clear();
            Console.WriteLine("List Menu");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. List of Students");
            Console.WriteLine("2. List of Trainers");
            Console.WriteLine("3. List of Courses");
            Console.WriteLine("4. List of Assignments");
            Console.WriteLine("5. List of Students per course");
            Console.WriteLine("6. List of Trainers per course");
            Console.WriteLine("7. List of Assignments per course");
            Console.WriteLine("8. List of Assignments per Students");
            Console.WriteLine("9. List of Students that belong to more than one courses");
            Console.WriteLine("10. Back");
            switch (Console.ReadLine())
            {
                case "1":
                    ShowStudents();
                    return true;
                case "2":
                    ShowTrainers();
                    return true;
                case "3":
                    ShowCourses();
                    return true;
                case "4":
                    ShowAssignments();
                    return true;
                case "5":
                    ShowStudentsPerCourse();
                    return true;
                case "6":
                    ShowTrainersPerCourse();
                    return true;
                case "7":
                    ShowAssignmentsPerCourse();
                    return true;
                case "8":
                    ShowAssignmentsPerCoursePerStudent();
                    return true;
                case "9":
                    ShowStudentsPerCourseMoreThanOneCourse();
                    return true;
                case "10":
                    Console.WriteLine("Back");
                    return false;
                default:
                    return true;
            }
        }

       

        private static void TheInputDataMenu()
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = ExteDataMenu();
            }
        }

        private static bool ExteDataMenu()
        {
            Console.Clear();
            Console.WriteLine("Input Menu");
            Console.WriteLine("1. Input Students");
            Console.WriteLine("2. Input Trainers");
            Console.WriteLine("3. Input Courses");
            Console.WriteLine("4. Input Assignments");
            Console.WriteLine("5. Set Students per course");
            Console.WriteLine("6. Set Trainers per course");
            Console.WriteLine("7. Set Assignments per Students per Course");
            Console.WriteLine("8. Back");
            switch (Console.ReadLine())
            {
                case "1":
                    InputStudents();
                    return true;
                case "2":
                    InputTrainers();
                    return true;
                case "3":
                    InputCourses();
                    return true;
                case "4":
                    InputAssignments();
                    return true;
                case "5":
                    SetStudentPerCourse();
                    return true;
                case "6":
                    SetTrainerPerCourse();
                    return true;
                case "7":
                    SetAssignmentPerStudentPerCourse();
                    return true;
                case "8":
                    Console.WriteLine("Back");
                    return false;
                default:
                    return true;
            }
        }
        #endregion


        static void Main(string[] args)
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }
        }
        
    }
}
