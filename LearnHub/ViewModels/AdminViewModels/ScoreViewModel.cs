using LearnHub.Models;
using LearnHub.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.ViewModels.AdminViewModels
{
    public class ScoreViewModel : BaseViewModel
    {
        
      private  Score _score;

    public event PropertyChangedEventHandler PropertyChanged;

    private string _regularScores;
    public string RegularScores
    {
        get => _regularScores;
        set
        {
          
                _regularScores = value;
                OnPropertyChanged(nameof(RegularScores));  // Thông báo thay đổi của TXScore
               OnPropertyChanged(nameof(AverageScore));  // Tính lại điểm trung bình khi TXScore thay đổi
            
        }
    }

    private double? _midTermScore;
    public double? MidTermScore
    {
        get => _midTermScore;
        set
        {
                _midTermScore = value;
                OnPropertyChanged(nameof(MidTermScore));
                OnPropertyChanged(nameof(AverageScore));  // Tính lại điểm trung bình khi GKScore thay đổi
            
        }
    }

    private double? _finalTermScore;
    public double? FinalTermScore
    {
        get => _finalTermScore;
        set
        {
              _finalTermScore = value;
                OnPropertyChanged(nameof(FinalTermScore));
                OnPropertyChanged(nameof(AverageScore));  // Tính lại điểm trung bình khi CKScore thay đổi
            
        }
    }

    public double AverageScore => CalculateAverageScore();
    public string StudentId => _score.StudentId;
    public Subject Subject { get; set; }
    public Student Student { get; set; }
    public AcademicYear AcademicYear { get; set; }

    public ScoreViewModel(Score score)
    {
        _score = score;
        _regularScores = score.RegularScores;  // Khởi tạo giá trị TXScore từ đối tượng Score ban đầu
        _midTermScore = score.MidTermScore;  // Khởi tạo giá trị GKScore từ đối tượng Score ban đầu
        _finalTermScore = score.FinalTermScore;  // Khởi tạo giá trị CKScore từ đối tượng Score ban đầu
        Include();
    }

  private async void Include()
    {
        Student = await GenericDataService<Student>.Instance.GetOne(e => e.Id == _score.StudentId);
        Subject = await GenericDataService<Subject>.Instance.GetOne(e => e.Id == _score.SubjectId);
        AcademicYear = await GenericDataService<AcademicYear>.Instance.GetOne(e => e.Id == _score.YearId);
    }

    public double CalculateAverageScore()
    {
        double sum = 0;
        int count = 0;

        // Tính trung bình điểm, bao gồm TXScore, GKScore, CKScore
        if (!string.IsNullOrWhiteSpace(RegularScores))
        {
            double[] txScores = RegularScores.Split(' ')
                                        .Select(s => double.Parse(s.Trim()))
                                        .ToArray();
            sum += txScores.Average();
            count++;
        }

        if (MidTermScore.HasValue)
        {
            sum += MidTermScore.Value;
            count++;
        }

        if (FinalTermScore.HasValue)
        {
            sum += FinalTermScore.Value;
            count++;
        }

        return count > 0 ? sum / count : 0;
    }

  
    public static IEnumerable<ScoreViewModel> ConvertToScoreViewModels(IEnumerable<Score> scores)
    {
        return scores.Select(score => new ScoreViewModel(score));
    }
}

}
