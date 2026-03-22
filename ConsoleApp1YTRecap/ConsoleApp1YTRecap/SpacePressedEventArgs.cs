using System;

 namespace ConsoleApp1YTRecap
 {
     // Custom EventArgs: subclass EventArgs to attach your own payload.
     // Naming convention: always end with "EventArgs".
     public class SpacePressedEventArgs : EventArgs
     {
         // The float value sent FROM the publisher TO the subscriber
         // with every spacebar press.
         public float Intensity { get; }

         // Constructor: publisher creates this object before firing the event
         public SpacePressedEventArgs(float intensity)
         {
             Intensity = intensity;
         }
     }
 }