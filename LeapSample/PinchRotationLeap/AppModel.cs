﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Media.Media3D;
using Monsoon.Reactive.Leap;
using Reactive.Bindings;

namespace PinchRotationLeap
{
    public class AppModel
    {
        const double MapScale = 0.05;

        public Transform3D CubeTransform { get; }
        QuaternionRotation3D quaternionRotation = new QuaternionRotation3D();
        TranslateTransform3D translateTransform = new TranslateTransform3D();

        public LeapManager LeapManager { get; } = new LeapManager();

        public ReadOnlyReactiveProperty<Leap.Hand> FrontHand { get; }
        public ReadOnlyReactiveProperty<Quaternion?> HandRotation { get; }
        public ReadOnlyReactiveProperty<Vector3D?> HandPosition { get; }
        public ReadOnlyReactiveProperty<bool> IsPinched { get; }

        public IObservable<Quaternion> PinchRotateDelta { get; }
        public ReactiveProperty<Quaternion> CubeRotation { get; } = new ReactiveProperty<Quaternion>(new Quaternion(new Vector3D(-0.8, 0.3, 0.5), 60));

        public IObservable<Vector3D> DragDelta { get; }
        public ReactiveProperty<Vector3D> CubePosition { get; } = new ReactiveProperty<Vector3D>();

        public AppModel()
        {
            CubeTransform = InitializeCubeTransform();

            var lastId = default(int?);
            FrontHand = LeapManager.FrameArrived
                .Select(f => f.Hands.Frontmost)
                .Select(h => h?.IsValid == true && (!lastId.HasValue || h.Id == lastId.Value) ? h : null)
                .Do(h => lastId = h?.Id)
                .ToReadOnlyReactiveProperty();
            HandRotation = FrontHand
                .Select(h => h?.GetEulerAngles().ToQuaternion())
                .ToReadOnlyReactiveProperty();
            HandPosition = FrontHand
                .Select(h => h?.PalmPosition.ToVector3D())
                .ToReadOnlyReactiveProperty();
            IsPinched = FrontHand
                .Select(h => h?.PinchStrength == 1.0F)
                .ToReadOnlyReactiveProperty();

            var lastHandRotation = Quaternion.Identity;
            PinchRotateDelta = IsPinched
                .Where(b => b)
                .Do(_ => lastHandRotation = HandRotation.Value.Value)
                .SelectMany(_ => HandRotation
                    .Where(q => q.HasValue)
                    .TakeWhile(q => IsPinched.Value)
                    .Select(q =>
                    {
                        lastHandRotation.Conjugate();
                        var d = q.Value * lastHandRotation;
                        lastHandRotation = q.Value;
                        return d;
                    }));

            PinchRotateDelta
                .Subscribe(d => CubeRotation.Value = d * CubeRotation.Value);
            CubeRotation
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(q => quaternionRotation.Quaternion = q);

            var lastHandPosition = default(Vector3D);
            DragDelta = IsPinched
                .Where(b => b)
                .Do(_ => lastHandPosition = HandPosition.Value.Value)
                .SelectMany(_ => HandPosition
                    .Where(v => v.HasValue)
                    .TakeWhile(v => IsPinched.Value)
                    .Select(v =>
                    {
                        var d = v.Value - lastHandPosition;
                        lastHandPosition = v.Value;
                        return d;
                    }));

            DragDelta
                .Subscribe(d => CubePosition.Value += MapScale * d);
            CubePosition
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(v =>
                {
                    translateTransform.OffsetX = v.X;
                    translateTransform.OffsetY = v.Y;
                    translateTransform.OffsetZ = v.Z;
                });
        }

        Transform3D InitializeCubeTransform()
        {
            var transform = new Transform3DGroup();
            transform.Children.Add(new RotateTransform3D(quaternionRotation));
            transform.Children.Add(translateTransform);
            return transform;
        }
    }

    public static class Leap3DHelper
    {
        public static Vector3D ToVector3D(this Leap.Vector v) => new Vector3D(v.x, v.y, v.z);

        // Improved values.
        public static EulerAngles GetEulerAngles(this Leap.Hand h) =>
            Rotation3DHelper.ToEulerAngles(-h.Direction.ToVector3D(), -h.PalmNormal.ToVector3D());
    }
}
