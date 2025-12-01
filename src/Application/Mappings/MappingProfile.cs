using AutoMapper;
using LearnHub.Back.Application.DTOs;
using LearnHub.Back.Application.Handlers.Course;
using LearnHub.Back.Application.Handlers.Student;
using LearnHub.Back.Application.Handlers.Enrollment;
using LearnHub.Back.Domain;

namespace LearnHub.Back.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Course mappings
            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title));
                
            CreateMap<CreateCourseCommand, Course>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name));
                
            CreateMap<UpdateCourseCommand, Course>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name));

            // Student mappings
            CreateMap<Student, StudentDto>();
            CreateMap<CreateStudentCommand, Student>();
            CreateMap<UpdateStudentCommand, Student>();

            // Enrollment mappings
            CreateMap<Enrollment, EnrollmentDto>()
                .ForMember(dest => dest.Student, opt => opt.MapFrom(src => src.Student))
                .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Course));
                
            CreateMap<CreateEnrollmentCommand, Enrollment>()
                .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(src => DateTime.UtcNow));
                
            CreateMap<UpdateEnrollmentCommand, Enrollment>()
                .ForMember(dest => dest.StudentId, opt => opt.Ignore())
                .ForMember(dest => dest.CourseId, opt => opt.Ignore())
                .ForMember(dest => dest.EnrollmentDate, opt => opt.Ignore());
        }
    }
}