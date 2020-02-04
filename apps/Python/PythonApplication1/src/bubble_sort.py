
def bubble_sort(unsorted):
	i = 0
	while i < len(unsorted) - 1:
		x = 1
		while x < len(unsorted) - i:
			if (unsorted[x - 1] > unsorted[x]):
				temp = unsorted[x - 1]
				unsorted[x - 1] = unsorted[x]
				unsorted[x] = temp
			x += 1
		i += 1
	return unsorted