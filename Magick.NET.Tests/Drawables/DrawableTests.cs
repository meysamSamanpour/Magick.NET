﻿//=================================================================================================
// Copyright 2013-2015 Dirk Lemstra <https://magick.codeplex.com/>
//
// Licensed under the ImageMagick License (the "License"); you may not use this file except in 
// compliance with the License. You may obtain a copy of the License at
//
//   http://www.imagemagick.org/script/license.php
//
// Unless required by applicable law or agreed to in writing, software distributed under the
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
// express or implied. See the License for the specific language governing permissions and
// limitations under the License.
//=================================================================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Magick.NET.Tests
{
	//==============================================================================================
	[TestClass]
	public class DrawableTests
	{
		//===========================================================================================
		private const string _Category = "DrawableTests";
		//===========================================================================================
		[TestMethod, TestCategory(_Category)]
		public void Test_Drawables()
		{
			Coordinate[] coordinates = new Coordinate[3];
			coordinates[0] = new Coordinate(0, 0);
			coordinates[1] = new Coordinate(50, 50);
			coordinates[2] = new Coordinate(99, 99);

			using (MagickImage image = new MagickImage(MagickColor.Transparent, 100, 100))
			{
				image.Draw(new DrawableAffine(0, 0, 1, 1, 2, 2));
				image.Draw(new DrawableArc(0, 0, 10, 10, 45, 90));
				image.Draw(new DrawableBezier(coordinates));
				image.Draw(new DrawableCircle(0, 0, 50, 50));
				image.Draw(new DrawableClipPath("foo"));
				image.Draw(new DrawableColor(0, 0, PaintMethod.Floodfill));

				using (MagickImage compositeImage = new MagickImage(new MagickColor("red"), 50, 50))
				{
					image.Draw(new DrawableCompositeImage(0, 0, compositeImage));
					image.Draw(new DrawableCompositeImage(0, 0, CompositeOperator.Over, compositeImage));
					image.Draw(new DrawableCompositeImage(new MagickGeometry(50, 50, 10, 10), compositeImage));
					image.Draw(new DrawableCompositeImage(new MagickGeometry(50, 50, 10, 10), CompositeOperator.Over, compositeImage));
				}

				image.Draw(new DrawableDashArray(new double[2] { 10, 20 }));
				image.Draw(new DrawableDashOffset(2));
				image.Draw(new DrawableEllipse(10, 10, 4, 4, 0, 360));
				image.Draw(new DrawableFillOpacity(0.5));
				image.Draw(new DrawableFillColor(Color.Red));
				image.Draw(new DrawableFont("Arial"));
				image.Draw(new DrawableGravity(Gravity.Center));
				image.Draw(new DrawableLine(20, 20, 40, 40));
				image.Draw(new DrawableMiterLimit(5));
				image.Draw(new DrawableOpacity(0, 0, PaintMethod.Floodfill));
				image.Draw(new DrawablePoint(60, 60));
				image.Draw(new DrawablePointSize(5));
				image.Draw(new DrawablePolygon(coordinates));
				image.Draw(new DrawablePolyline(coordinates));
				image.Draw(new DrawableRectangle(30, 30, 70, 70));
				image.Draw(new DrawableRotation(180));
				image.Draw(new DrawableRoundRectangle(50, 50, 30, 30, 70, 70));
				image.Draw(new DrawableScaling(15, 15));
				image.Draw(new DrawableSkewX(90));
				image.Draw(new DrawableSkewY(90));
				image.Draw(new DrawableStrokeAntialias(true));
				image.Draw(new DrawableStrokeColor(Color.Purple));
				image.Draw(new DrawableStrokeLineCap(LineCap.Square));
				image.Draw(new DrawableStrokeLineJoin(LineJoin.Bevel));
				//image.Draw(new DrawableStrokeOpacity(0.8));
				image.Draw(new DrawableStrokeWidth(4));
				image.Draw(new DrawableText(0, 60, "test"));
				image.Draw(new DrawableTextAntialias(true));
				image.Draw(new DrawableTextDecoration(TextDecoration.LineThrough));
				image.Draw(new DrawableTextDirection(TextDirection.RightToLeft));
				image.Draw(new DrawableTextInterlineSpacing(4));
				image.Draw(new DrawableTextInterwordSpacing(6));
				image.Draw(new DrawableTextKerning(2));
				image.Draw(new DrawableTextUnderColor(Color.Yellow));
				image.Draw(new DrawableTranslation(65, 65));
				image.Draw(new DrawableViewbox(0, 0, 100, 100));

				image.Draw(new DrawablePushClipPath("#1"), new DrawablePopClipPath());
				image.Draw(new DrawablePushPattern("test", 30, 30, 10, 10), new DrawablePopPattern());
				image.Draw(new DrawablePushGraphicContext(), new DrawablePopGraphicContext());
			}
		}
		//===========================================================================================
		[TestMethod, TestCategory(_Category)]
		public void Test_Drawables_Exceptions()
		{
			ExceptionAssert.Throws<ArgumentException>(delegate()
			{
				new DrawableBezier();
			});

			ExceptionAssert.Throws<ArgumentNullException>(delegate()
			{
				new DrawableBezier(null);
			});

			ExceptionAssert.Throws<ArgumentException>(delegate()
			{
				new DrawableBezier(new Coordinate[] { });
			});

			ExceptionAssert.Throws<ArgumentException>(delegate()
			{
				new DrawablePolygon(new Coordinate[] { new Coordinate(0, 0) });
			});

			ExceptionAssert.Throws<ArgumentException>(delegate()
			{
				new DrawablePolyline(new Coordinate[] { new Coordinate(0, 0), new Coordinate(0, 0) });
			});
		}
		//===========================================================================================
		[TestMethod, TestCategory(_Category)]
		public void Test_DrawablePaths()
		{
			using (MagickImage image = new MagickImage(MagickColor.Transparent, 100, 100))
			{
				List<IPath> paths = new List<IPath>();
				paths.Add(new PathArcAbs(new PathArc(50, 50, 20, 20, 45, true, false)));
				paths.Add(new PathArcRel(new PathArc(10, 10, 5, 5, 40, false, true)));
				paths.Add(new PathClosePath());
				paths.Add(new PathCurvetoAbs(new PathCurveto(80, 80, 10, 10, 60, 60)));
				paths.Add(new PathCurvetoRel(new PathCurveto(30, 30, 60, 60, 90, 90)));
				paths.Add(new PathLinetoAbs(new Coordinate(70, 70)));
				paths.Add(new PathLinetoHorizontalAbs(20));
				paths.Add(new PathLinetoHorizontalRel(90));
				paths.Add(new PathLinetoRel(new Coordinate(0, 0)));
				paths.Add(new PathLinetoVerticalAbs(70));
				paths.Add(new PathLinetoVerticalRel(30));
				paths.Add(new PathMovetoAbs(new Coordinate(50, 50)));
				paths.Add(new PathMovetoRel(new Coordinate(20, 20)));
				paths.Add(new PathQuadraticCurvetoAbs(new PathQuadraticCurveto(70, 70, 30, 30)));
				paths.Add(new PathQuadraticCurvetoRel(new PathQuadraticCurveto(10, 10, 40, 40)));
				paths.Add(new PathSmoothCurvetoAbs(new Coordinate(0, 0), new Coordinate(30, 30)));
				paths.Add(new PathSmoothCurvetoRel(new Coordinate(60, 60), new Coordinate(10, 10)));
				paths.Add(new PathSmoothQuadraticCurvetoAbs(new Coordinate(50, 50)));
				paths.Add(new PathSmoothQuadraticCurvetoRel(new Coordinate(80, 80)));

				image.Draw(new DrawablePath(paths));
			}
		}
		//===========================================================================================
		[TestMethod, TestCategory(_Category)]
		public void Test_DrawablePath_Exceptions()
		{
			ExceptionAssert.Throws<ArgumentException>(delegate()
			{
				new PathArcAbs();
			});

			ExceptionAssert.Throws<ArgumentNullException>(delegate()
			{
				new PathArcAbs(null);
			});

			ExceptionAssert.Throws<ArgumentException>(delegate()
			{
				new PathArcAbs(new PathArc[] { });
			});

			ExceptionAssert.Throws<ArgumentNullException>(delegate()
			{
				new PathArcAbs(new PathArc[] { null });
			});

			ExceptionAssert.Throws<ArgumentException>(delegate()
			{
				new PathSmoothCurvetoAbs(new Coordinate[] { new Coordinate(0, 0) });
			});
		}
		//===========================================================================================
	}
	//==============================================================================================
}