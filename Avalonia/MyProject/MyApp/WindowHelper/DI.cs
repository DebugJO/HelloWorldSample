using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace MyApp.WindowHelper;

public static class DI
{
    // Default 함수
    // public static T Get<T>() where T : notnull
    //     => App.Services.GetRequiredService<T>();
    //
    // public static object Get(Type type)
    //     => App.Services.GetRequiredService(type);

    /// <summary>
    /// 현재 스레드에서 생성 중인 타입들을 추적 (순환 참조 감지용)
    /// </summary>
    private static readonly ThreadLocal<HashSet<Type>> _resolutionStack = new(() => []);

    /// <summary>
    /// 지정된 타입에 해당하는 의존성 객체(인스턴스) 가져오기.
    /// <![CDATA[
    /// CurrentPage = DI.Get<MainViewModel>();
    /// ]]>
    /// </summary>
    /// <typeparam name="T">가져올 객체 타입 : View, ViewModel, Service</typeparam>
    /// <returns>컨테이터에 등록된 [T]의 싱글톤, 새 인스턴스 : View, ViewModel, Service</returns>
    /// <exception cref="InvalidOperationException">의존성 주입 과정에서 순환 참조가 발생하거나 등록되지 않은 타입을 요청할 때 발생</exception>
    public static T Get<T>() where T : notnull
    {
        Type type = typeof(T);

        if (_resolutionStack.Value!.Contains(type))
        {
            throw new InvalidOperationException(
                $"[순환 참조] {type.Name}이(가) 생성 완료되기 전에 재요청 감지. 생성자 내부의 DI 호출을 확인하세요."
            );
        }

        try
        {
            _resolutionStack.Value.Add(type);
            return App.Services.GetRequiredService<T>();
        }
        finally
        {
            _resolutionStack.Value.Remove(type);
        }
    }

    /// <summary>
    /// 타입(이름)으로  의존성 객체(인스턴스) 가져오기.
    /// 메뉴 버튼이 여러 개인데, 버튼마다 함수를 만들지 않고 CommandParameter에 이름만 넣어서 처리
    /// 파라미터로 뷰모델 이름의 앞부분만 전달 : CommandParameter="ServerConfig", Command는 동일
    /// <![CDATA[
    /// var typeName = $"MyApp.ViewModels.{formName}ViewModel";
    /// var type = Type.GetType(typeName);
    /// CurrentPage = DI.Get<MainViewModel>();
    /// ]]>
    /// </summary>
    /// <param name="type">가져올 객체 타입 : View, ViewModel, Service</param>
    /// <returns>컨테이터에 등록된 [T]의 싱글톤, 새 인스턴스 : View, ViewModel, Service</returns>
    /// <exception cref="InvalidOperationException">순환참고, 동적 요청 중 무한 루프 감지</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static object Get(Type type)
    {
        if (_resolutionStack.Value!.Contains(type))
        {
            throw new InvalidOperationException(
                $"[순환 참조] {type.Name} 동적 요청 중 무한 루프 감지. 생성자 내부의 DI 호출을 확인하세요."
            );
        }

        try
        {
            _resolutionStack.Value.Add(type);
            return App.Services.GetRequiredService(type);
        }
        finally
        {
            _resolutionStack.Value.Remove(type);
        }
    }
}

/* 블록 안의 코드는 컴파일 시점에 포함 안 함
#if DISABLE_GET_METHOD
public static object Get(Type type) { ... }
#endif
*/