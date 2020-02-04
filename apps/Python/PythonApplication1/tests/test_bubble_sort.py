import unittest

from src.bubble_sort import bubble_sort

class TestBubbleSort(unittest.TestCase):

    def test_empty_array(self):
        self.assertEqual(bubble_sort([]), [])

    def test_one_item(self):
        self.assertEqual(bubble_sort([0]), [0])

    def test_two_item(self):
        self.assertEqual(bubble_sort([1, 0]), [0, 1])

    def test_three_item(self):
        self.assertEqual(bubble_sort([2, 1, 0]), [0, 1, 2])

    def test_four_item(self):
        self.assertEqual(bubble_sort([3, 2, 1, 0]), [0, 1, 2, 3])


if __name__ == '__main__':
    unittest.main()