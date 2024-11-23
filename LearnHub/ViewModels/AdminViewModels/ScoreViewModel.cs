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
    public class ScoreViewModel : BaseViewModel, INotifyPropertyChanged
    {
        //    private readonly Score _score;
        //    public string StudentId => _score.StudentId;
        //    public double? GKScore => _score.GKScore;
        //    public double? CKScore => _score.CKScore;
        //    public string? TXScore => _score.TXScore;
        //    public double AverageScore => CalculateAverageScore();
        //    public Subject Subject { get; set; }
        //    public Student Student { get; set; }
        //    public AcademicYear AcademicYear { get; set; }

        //    ScoreViewModel(Score score)
        //    {
        //        _score = score;
        //        Include();
        //    }
        //    async void Include()
        //    {
        //        Student = await GenericDataService<Student>.Instance.GetOne(e => e.Id == _score.StudentId);
        //        Subject = await GenericDataService<Subject>.Instance.GetOne(e => e.Id == _score.SubjectId);
        //        AcademicYear = await GenericDataService<AcademicYear>.Instance.GetOne(e => e.Id == _score.YearId);
        //    }
        //    public double CalculateAverageScore()
        //    {
        //        if (string.IsNullOrWhiteSpace(TXScore)) return 0;

        //        // Tách chuỗi bằng dấu cách và chuyển đổi thành mảng double
        //        double[] numbers = TXScore.Split(' ')
        //                                .Select(s => double.Parse(s.Trim()))
        //                                .ToArray();

        //        // Tính trung bình
        //        return numbers.Average();
        //    }
        //    public static IEnumerable<ScoreViewModel> ConvertToScoreViewModels(IEnumerable<Score> scores)
        //    {
        //        return scores.Select(score => new ScoreViewModel(score));
        //    }
        private  Score _score;

    public event PropertyChangedEventHandler PropertyChanged;

    private string _txScore;
    public string TXScore
    {
        get => _txScore;
        set
        {
            if (_txScore != value)
            {
                _txScore = value;
                OnPropertyChanged(nameof(TXScore));  // Thông báo thay đổi của TXScore
                OnPropertyChanged(nameof(AverageScore));  // Tính lại điểm trung bình khi TXScore thay đổi
            }
        }
    }

    private double? _gkScore;
    public double? GKScore
    {
        get => _gkScore;
        set
        {
            if (_gkScore != value)
            {
                _gkScore = value;
                OnPropertyChanged(nameof(GKScore));
                OnPropertyChanged(nameof(AverageScore));  // Tính lại điểm trung bình khi GKScore thay đổi
            }
        }
    }

    private double? _ckScore;
    public double? CKScore
    {
        get => _ckScore;
        set
        {
            if (_ckScore != value)
            {
                _ckScore = value;
                OnPropertyChanged(nameof(CKScore));
                OnPropertyChanged(nameof(AverageScore));  // Tính lại điểm trung bình khi CKScore thay đổi
            }
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
        _txScore = score.TXScore;  // Khởi tạo giá trị TXScore từ đối tượng Score ban đầu
        _gkScore = score.GKScore;  // Khởi tạo giá trị GKScore từ đối tượng Score ban đầu
        _ckScore = score.CKScore;  // Khởi tạo giá trị CKScore từ đối tượng Score ban đầu
        Include();
    }

    async void Include()
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
        if (!string.IsNullOrWhiteSpace(TXScore))
        {
            double[] txScores = TXScore.Split(' ')
                                        .Select(s => double.Parse(s.Trim()))
                                        .ToArray();
            sum += txScores.Average();
            count++;
        }

        if (GKScore.HasValue)
        {
            sum += GKScore.Value;
            count++;
        }

        if (CKScore.HasValue)
        {
            sum += CKScore.Value;
            count++;
        }

        return count > 0 ? sum / count : 0;
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public static IEnumerable<ScoreViewModel> ConvertToScoreViewModels(IEnumerable<Score> scores)
    {
        return scores.Select(score => new ScoreViewModel(score));
    }
}

}
