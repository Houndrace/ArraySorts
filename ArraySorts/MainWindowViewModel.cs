using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace ArraySorts
{
    partial class MainWindowViewModel : ObservableValidator
    {
        private const int MinNumberValueForArray = 1;
        private const int MaxNumberValueForArray = 100;

        private int[]? array;

        [ObservableProperty]
        private string? generatedArrayResult;
        [ObservableProperty]
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
        private string? sortedArrayResult;


        [RelayCommand]
        public void GenerateNumberArray()
        {
            array = new int[Length];
            Random random = new Random();

            for (int i = 0; i < Length; i++)
            {
                array[i] = random.Next(MinNumberValueForArray, MaxNumberValueForArray);
            }

            GeneratedArrayResult = string.Join(", ", array);
        }

        [RelayCommand]
        public void SortNumberArray()
        {
            if (array == null)
                return;

            Sorter sorter = new Sorter();

            if (IsBubbleSortChecked)
            {
                sorter.BubbleSort(array);
            }
            else if (IsInsertionSortChecked)
            {
                sorter.InsertionSort(array);
            }
            else if (IsSelectionSortChecked)
            {
                sorter.SelectionSort(array);
            }
            else if (IsQuickSortChecked)
            {
                array = sorter.QuickSort(array, 0, array.Length - 1);
            }
            else if (IsMergeSortChecked)
            {
                array = sorter.MergeSort(array);
            }
            else if (IsShakerSortChecked)
            {
                array = sorter.ShakerSort(array);
            }

            SortedArrayResult = string.Join(", ", array);
        }
    }

    public class Sorter
    {
        public void BubbleSort(int[] arr)
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
        }

        public void InsertionSort(int[] arr)
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
        }

        public void SelectionSort(int[] arr)
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



