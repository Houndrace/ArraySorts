using ArraySorts.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace ArraySorts
{
    partial class MainWindowViewModel : ObservableValidator
    {
        private const int MinNumberValueForArray = 1;
        private const int MaxNumberValueForArray = 100000;
        private const int EvaluateRepetitionsNumber = 5;

        private ArraySortingContext db = new();
        private int[]? generatedArray;
        private int[]? sortedArray;
        private string? sortingTypeName;

        [ObservableProperty]
        private string? generatedStringResult;
        [ObservableProperty]
        [Range(2, MaxNumberValueForArray, ErrorMessage = "Необходимо ввести число в пределах от 2 до 100000")]
        private int length;

        [ObservableProperty]
        private bool isBubbleSortChecked = true;
        [ObservableProperty]
        private bool isInsertionSortChecked = false;
        [ObservableProperty]
        private bool isSelectionSortChecked = false;
        [ObservableProperty]
        private bool isQuickSortChecked = false;
        [ObservableProperty]
        private bool isMergeSortChecked = false;
        [ObservableProperty]
        private bool isShakerSortChecked = false;

        [ObservableProperty]
        private string? sortedStringResult;


        [RelayCommand]
        public void GenerateNumberArray()
        {
            ClearErrors();
            ValidateAllProperties();

            if (HasErrors)
            {
                MessageBox.Show("fadsf");
                return;
            }

            generatedArray = new int[Length];
            Random random = new Random();

            for (int i = 0; i < Length; i++)
            {
                generatedArray[i] = random.Next(MinNumberValueForArray, MaxNumberValueForArray);
            }

            GeneratedStringResult = string.Join(", ", generatedArray);
        }

        [RelayCommand]
        public void SortNumberArray()
        {
            if (generatedArray == null)
                return;

            int[] tempGeneratedArray = new int[Length];
            generatedArray.CopyTo(tempGeneratedArray, 0);
            Sorter sorter = new();

            if (IsBubbleSortChecked)
            {
                sortedArray = sorter.BubbleSort(tempGeneratedArray);
                sortingTypeName = "Пузырьковая";
            }
            else if (IsInsertionSortChecked)
            {
                sortedArray = sorter.InsertionSort(tempGeneratedArray);
                sortingTypeName = "Вставкой";
            }
            else if (IsSelectionSortChecked)
            {
                sortedArray = sorter.SelectionSort(tempGeneratedArray);
                sortingTypeName = "Выбором";
            }
            else if (IsQuickSortChecked)
            {
                sortedArray = sorter.QuickSort(tempGeneratedArray, 0, tempGeneratedArray.Length - 1);
                sortingTypeName = "Быстрая";
            }
            else if (IsMergeSortChecked)
            {
                sortedArray = sorter.MergeSort(tempGeneratedArray);
                sortingTypeName = "Слиянием";
            }
            else if (IsShakerSortChecked)
            {
                sortedArray = sorter.ShakerSort(tempGeneratedArray);
                sortingTypeName = "Шейкерная";
            }

            if (sortedArray != null)
                SortedStringResult = string.Join(", ", sortedArray);
        }

        [RelayCommand]
        public void Clear()
        {
            GeneratedStringResult = null;
            SortedStringResult = null;
            generatedArray = null;
            Length = 0;
        }

        [RelayCommand]
        public void EvaluatePerformance()
        {
            if (generatedArray is null)
            {
                MessageBox.Show("Необходимо сгенерировать массив перед оценкой", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            Stopwatch stopwatch = new();
            List<long> sortDelayList = new();

            for (int i = 0; i < EvaluateRepetitionsNumber; i++)
            {
                stopwatch.Restart();
                SortNumberArray();
                stopwatch.Stop();
                sortDelayList.Add(stopwatch.ElapsedMilliseconds);
            }


            AddRecordToDB(generatedArray, sortingTypeName, (int)sortDelayList.Min());
        }

        private void AddRecordToDB(int[] originalIntArray, string sortingTypeName, int bestTimeResult)
        {
            SortingType sortingType = db.SortingTypes.Where(type => type.TypeName == sortingTypeName).FirstOrDefault();

            string originalStringArray = string.Join(',', originalIntArray);

            Models.Array originalArray = db.Arrays.Where(array => array.Data == originalStringArray).FirstOrDefault();

            if (originalArray is null)
            {
                originalArray = new()
                {
                    Data = originalStringArray,
                    ItemQuantity = originalIntArray.Length,
                };
            }

            if (db.Sortings.Include(sorting => sorting.Type).FirstOrDefault(sorting => sorting.Type == sortingType) is not null
                && db.Sortings.Include(sorting => sorting.OriginalArray).FirstOrDefault(sorting => sorting.OriginalArray.ItemQuantity == originalIntArray.Length) is not null)
            {
                return;
            }





            Sorting sorting = new()
            {
                OriginalArray = originalArray,
                StartDate = DateTime.Now,
                StartTime = DateTime.Now.TimeOfDay,
                TimeResult = bestTimeResult,
                Type = sortingType
            };

            db.Sortings.Add(sorting);
            db.SaveChanges();
        }


    }

    public class Sorter
    {
        public int[] BubbleSort(int[] arr)
        {
            int n = arr.Length;
            bool swapped;

            for (int i = 0; i < n - 1; i++)
            {
                swapped = false;

                for (int j = 0; j < n - i - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        // Swap arr[j] and arr[j+1]
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                        swapped = true;
                    }
                }

                // If no two elements were swapped in the inner loop, the array is already sorted
                if (!swapped)
                {
                    break;
                }
            }

            return arr;
        }

        public int[] InsertionSort(int[] arr)
        {
            int n = arr.Length;

            for (int i = 1; i < n; i++)
            {
                int key = arr[i];
                int j = i - 1;

                // Move elements of arr[0..i-1] that are greater than key
                // to one position ahead of their current position
                while (j >= 0 && arr[j] > key)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }

                arr[j + 1] = key;
            }

            return arr;
        }

        public int[] SelectionSort(int[] arr)
        {
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;

                // Find the minimum element in the unsorted part of the array
                for (int j = i + 1; j < n; j++)
                {
                    if (arr[j] < arr[minIndex])
                    {
                        minIndex = j;
                    }
                }

                // Swap the found minimum element with the first element
                int temp = arr[i];
                arr[i] = arr[minIndex];
                arr[minIndex] = temp;
            }

            return arr;
        }

        public int[] QuickSort(int[] arr, int low, int high)
        {
            if (low < high)
            {
                // Partition the array into two subarrays
                int pivotIndex = Partition(arr, low, high);

                // Recursively sort the subarrays
                QuickSort(arr, low, pivotIndex - 1);
                QuickSort(arr, pivotIndex + 1, high);
            }
            return arr;
        }

        private int Partition(int[] arr, int low, int high)
        {
            int pivot = arr[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (arr[j] < pivot)
                {
                    i++;

                    // Swap arr[i] and arr[j]
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }

            // Swap arr[i+1] and arr[high] (pivot)
            int temp2 = arr[i + 1];
            arr[i + 1] = arr[high];
            arr[high] = temp2;

            return i + 1;
        }

        public int[] MergeSort(int[] arr)
        {
            if (arr.Length <= 1)
            {
                return arr;
            }

            int middle = arr.Length / 2;
            int[] left = new int[middle];
            int[] right = new int[arr.Length - middle];

            for (int i = 0; i < middle; i++)
            {
                left[i] = arr[i];
            }
            for (int i = middle; i < arr.Length; i++)
            {
                right[i - middle] = arr[i];
            }

            left = MergeSort(left);
            right = MergeSort(right);

            return Merge(left, right);
        }
        private int[] Merge(int[] left, int[] right)
        {
            int[] result = new int[left.Length + right.Length];
            int leftIndex = 0, rightIndex = 0, resultIndex = 0;

            while (leftIndex < left.Length && rightIndex < right.Length)
            {
                if (left[leftIndex] <= right[rightIndex])
                {
                    result[resultIndex] = left[leftIndex];
                    leftIndex++;
                }
                else
                {
                    result[resultIndex] = right[rightIndex];
                    rightIndex++;
                }
                resultIndex++;
            }

            while (leftIndex < left.Length)
            {
                result[resultIndex] = left[leftIndex];
                leftIndex++;
                resultIndex++;
            }

            while (rightIndex < right.Length)
            {
                result[resultIndex] = right[rightIndex];
                rightIndex++;
                resultIndex++;
            }

            return result;
        }

        public int[] ShakerSort(int[] arr)
        {
            int left = 0;
            int right = arr.Length - 1;
            bool swapped;

            do
            {
                swapped = false;

                // Perform a pass from left to right (bubble sort)
                for (int i = left; i < right; i++)
                {
                    if (arr[i] > arr[i + 1])
                    {
                        // Swap arr[i] and arr[i+1]
                        int temp = arr[i];
                        arr[i] = arr[i + 1];
                        arr[i + 1] = temp;
                        swapped = true;
                    }
                }

                if (!swapped)
                {
                    break; // If no swaps occurred in this pass, the array is sorted
                }

                right--;

                // Perform a pass from right to left (bubble sort in reverse)
                for (int i = right; i > left; i--)
                {
                    if (arr[i] < arr[i - 1])
                    {
                        // Swap arr[i] and arr[i-1]
                        int temp = arr[i];
                        arr[i] = arr[i - 1];
                        arr[i - 1] = temp;
                        swapped = true;
                    }
                }

                left++;
            } while (swapped);
            return arr;
        }
    }
}



